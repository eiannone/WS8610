using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using WS8610;

namespace WS8610Recorder
{
	public enum LogLevel
	{
		Disable = 0,
		Error = 1,
		Warning = 2,
		Info = 3,
		Debug = 4,
		Trace = 5
	}

	public struct Notify
	{
		public DateTime DateTime;
		public string Message;
	}

	internal enum Dst { Allineato, SolareLegale, LegaleSolare }

	public class Recorder
	{
		const int M_STEP = 5; // Intervallo di misurazione, in minuti
		const double MAX_TEMP_LIMIT = 45.0; // Limite alla massima temperatura attesa
		readonly static TimeSpan T_STEP = TimeSpan.FromMinutes(M_STEP);
		private static readonly string NOTIFY_PATH = Config.LogFolder + "\\notify";
		private readonly LogLevel _logLevel = Config.LogLevel;
		private readonly bool _isInternalLogger;
		private TextWriter _logger;
		private bool _internalLoggerOpen;
		private readonly WS8610Com _wscom;

		public event ProgressEventHandler OnProgress;

		public static string LogPath => GetLogFile(DateTime.Now);

	    private bool LogIsOpen => !_isInternalLogger || _internalLoggerOpen;

	    public Recorder(TextWriter logger = null) {
			_isInternalLogger = (logger == null);
			_logger = logger;
			_wscom = new WS8610Com(Config.ComPort, _logger);
			//Log("Microdelay ticks: " + WS8610Com.microdelay_ticks, LogLevel.Trace);
		}

		public DateTime GetLastRecordDateTime() {
			if (Config.LastDt.Year == 2004) {
				var db = new DaoMeasurements();
				return db.GetLastRecord().DateTime;
			}
			return Config.LastDt;
		}

		public bool DownloadNewData(bool reportProgress = false) {
			open_log();
			Log("Ultimo record scaricato: #" + Config.LastMem.Index + " " + Config.LastMem.DateTime.ToString("dd/MM/yyyy HH:mm"));

			if (reportProgress) _wscom.OnProgress += wscom_OnProgress;
			try {
				var crono = Stopwatch.StartNew();
				open_station();
				var lastMem = read_record(Config.LastMem.Index);
				if (!lastMem.IsValid || lastMem.DateTime != Config.LastMem.DateTime) {
					var msg = lastMem.IsValid ? lastMem.DateTimeStr : "record #" + lastMem.Index + " non valido";
					throw new Exception("Dati sul db non congruenti con quelli nella stazione meteo (" + msg + ")");
				}
				if (lastMem.IsLast) {
					throw new Exception("Nessun nuovo dato disponibile nella stazione meteo.");
				}
				var recList = read_new_records((lastMem.Index < _wscom.MaxHistoryRecords - 1) ? lastMem.Index + 1 : 0);
				var elapsed = crono.Elapsed;
				crono.Reset();
				Log("Tempo impiegato a leggere " + recList.Count + " record: " + elapsed.ToString(@"m\:ss\.fff"));
				Notify("Rilevati " + recList.Count + " nuovi record.");

				if (recList.Count == 0) return true;
				var lastHr = recList.Last();

				// Verifica disallineamento dell'orario della stazione meteo con quello del PC
				var curDt = DateTime.Now.Subtract(TimeSpan.FromMinutes(DateTime.Now.Minute % M_STEP));
				var disallin = (int)(lastHr.DateTime - curDt).TotalMinutes;
				if (Math.Abs(disallin) > M_STEP) {
					var txtDisall = (lastHr.DateTime - curDt).ToString(@"h\:mm");
					if (Math.Abs(disallin) >= 24 * 60) {
						txtDisall = "Stazione = " + lastHr.DateTime + " / PC = " + curDt;
					}
					Log("Attenzione: disallineamento orari Stazione/PC: " + txtDisall, LogLevel.Warning);
					if (Math.Abs(disallin) > 70) throw new Exception("Disallineamento Stazione/PC oltre i limiti");
				}

				// Converte gli HistoryRecord in Measure, effettuando una prima verifica sui dati
				var mList = get_measurements(recList, lastMem.DateTime);

				// Verifica se all'interno dei record estratti cade un cambio di ora legale / solare
				// oppure se dopo il cambio l'orario della stazione non è stato ancora aggiornato
				DateTime dtDst;
				var statoDst = rileva_stato_dst(mList[0].DateTime, lastHr.DateTime, disallin, out dtDst);

				var skipped_1H = false; // Passa a true quando viene rilevato un salto di 1 ora negli orari registrati
				var dtPrec = lastMem.DateTime;
                var numSens = mList[0].Sensor.Length;
                var tPrec = new decimal?[numSens];
                for (var s = 0; s < tPrec.Length; s++) {
                    tPrec[s] = lastMem.HasTemp(s) ? (decimal?)lastMem.Temp[s] : null;
                }
				foreach (var m in mList) {
                    // Verifica variazioni troppo brusche di temperatura su un sensore
                    for (var s = 0; s < numSens; s++) {
                        if (tPrec[s] != null && m.Sensor[s].Temp != null && Math.Abs((decimal)m.Sensor[s].Temp - (decimal)tPrec[s]) > 3) {
                            var dati = "Sensore " + s + ", record del " + m.DateTimeStr + ". Valori: " + tPrec[s] + " e " + m.Sensor[s].Temp;
                            throw new Exception("Differenza elevata di temperatura tra due misurazioni consecutive. " + dati);
                        }
                        tPrec[s] = m.Sensor[s].Temp;
                    }

                    // Verifica orari sballati (tipicamente quando si scarica la batteria in un sensore)
                    // Controlla che la differenza tra esterno nord (1) e sud (3) e tra interno camera (2) 
                    // e cameretta (0) non sia superiore a 7 gradi
                    int[][] coppieSensori = {new int[]{ 1, 3 }, new int[]{ 0, 2}};
                    foreach (var s in coppieSensori) {
                        var t1 = m.Sensor[s[0]].Temp;
                        var t2 = m.Sensor[s[1]].Temp;
                        if (t1 != null && t2 != null && Math.Abs((double)t1 - (double)t2) > 7.0) {
                            throw new Exception("Differenza elevata di temperatura tra sensori " + s[0] + " e " + s[1] + ". " +
                                "Record del " + m.DateTimeStr + ". Valori: " + t1 + " e " + t2);
                        }
                    }
                    
					// Verifica salti di orario
					var diffMin = (int)Math.Abs((m.DateTime - dtPrec).TotalMinutes);
					if (diffMin > 50 && diffMin < 70) skipped_1H = true;
					if (diffMin > M_STEP && (statoDst == Dst.Allineato || !skipped_1H)) throw new Exception("Record non contigui");

					dtPrec = m.DateTime;
					// Verifica e corregge variazioni ora solare/legale
					switch (statoDst) {
						case Dst.SolareLegale:
							// Se è stata superata l'ora di passaggio all'ora legale, ma l'ora non è stata aggiornata,
							// allora la aggiorna manualmente.
							if (!skipped_1H && m.DateTime >= dtDst) m.DateTime = m.DateTime.AddHours(1);
							break;
						case Dst.LegaleSolare:
							// Se è stata superata l'ora di passaggio all'ora solare, ma l'ora non è stata aggiornata,
							// allora la aggiorna manualmente. L'ora 2-3 viene forzata manualmente a ora legale.
							// Gli orari successivi vengono scalati di un'ora.
							if (!skipped_1H && m.DateTime >= dtDst.AddHours(-1)) {
								if (m.DateTime < dtDst) m.IsDaylightSaving = true;
								else m.DateTime = m.DateTime.AddHours(-1);
							}
							break;
					}
				}

				crono.Start();
				var db = new DaoMeasurements();
				db.InsertHistoryRecords(mList);
				Config.LastMem = lastHr;
				Config.LastDt = mList.Last().DateTime;
                Config.DstAlign = !((disallin > 50 && disallin < 70) || (disallin > -70 && disallin < -50));

				// Resetta contatore memoria
				reset_counter(0);

				// Aggiorna statistiche
				update_stats();
			}
			catch (Exception ex) {
				Log("ERRORE: " + ex.Message + " Stacktrace: " + ex.StackTrace, LogLevel.Error);
				return false;
			}
			finally {
				close_station();
				close_log();
			}
			return true;
		}

		private List<Measure> get_measurements(IList<HistoryRecord> recList, DateTime? precDt = null) {
			var ml = new List<Measure>(recList.Count);
			if (recList.Count == 0) return ml;

			var nSens = recList[0].Temp.Length; // Numero di sensori misurati
			var noTemps = new int[nSens];
			var noHums = new int[nSens];

			Log("Primo record: #" + recList[0].Index + " - " + recList[0].DateTimeStr, LogLevel.Debug);
			var lastRec = recList.Last();
			Log("Ultimo record: #" + lastRec.Index + " - " + lastRec.DateTimeStr, LogLevel.Debug);

			var dtPrec = precDt ?? recList[0].DateTime.AddMinutes(-M_STEP);
			foreach (var hr in recList) {
				Log(hr.DateTimeStr, LogLevel.Trace);
				if (!hr.IsValid) {
					Log("Attenzione: record #" + hr.Index + " non valido.");
					continue;
				}
				var m = new Measure(hr.DateTime);
				for (var s = 0; s < hr.Temp.Length; s++) {
					if (hr.HasTemp(s) && hr.Temp[s] < MAX_TEMP_LIMIT) m.Sensor[s].Temp = (decimal?)hr.Temp[s];
					if (hr.HasHum(s)) m.Sensor[s].Humid = hr.Hum[s];
				}

				// Corregge record con orario non ai 5 minuti
				if (m.DateTime.Minute % M_STEP != 0) {
					var newDt = m.DateTime.Subtract(TimeSpan.FromMinutes(m.DateTime.Minute % M_STEP));
					if (newDt == dtPrec) newDt = newDt.AddMinutes(M_STEP);
					Log("Corretto record con orario anomalo: " + m.DateTimeStr + " -> " + newDt.ToString("HH:mm"), LogLevel.Debug);
					m.DateTime = newDt;
				}

				// Verifica registrazioni non contigue
				var diffMin = (int)Math.Abs((m.DateTime - dtPrec).TotalMinutes);
				if (diffMin != 0 && diffMin != M_STEP) {
					var dt2 = (dtPrec.Date == m.DateTime.Date) ? m.DateTime.ToString("HH:mm") : m.DateTimeStr;
					Log("Attenzione: registrazioni non contigue: " + dtPrec.ToString("dd/MM/yyyy HH:mm") + " -> " + dt2, LogLevel.Warning);
				}

				// Verifica date duplicate
				if (diffMin == 0) {
					Log("Attenzione: record duplicati al " + m.DateTime + ". Verrà mantenuto solo il primo.", LogLevel.Warning);
				}
				else {
					// Conteggio record nulli
					for (var s = 0; s < nSens; s++) {
						if (m.Sensor[s].Temp == null) noTemps[s]++;
						if (m.Sensor[s].Humid == null) noHums[s]++;
					}
					ml.Add(m);
				}
				dtPrec = m.DateTime;
			}

			// Logga i record nulli
			var str = "";
			for (var s = 0; s < nSens; s++) {
				if (noTemps[s] + noHums[s] == 0) continue;
				var percT = Math.Round((decimal)noTemps[s] * 100 / ml.Count, 1);
				var percU = Math.Round((decimal)noHums[s] * 100 / ml.Count, 1);
				str += "[Sensore" + s + ": " +
							 "temp. = " + noTemps[s] + " (" + percT + "%), umid. = " + noHums[s] + " (" + percU + "%)] ";
				if (percT > 10) Notify("Nulli Sensore" + s + ": " + percT + "% (" + noTemps[s] + ")");
			}
			if (str != "") Log("Valori nulli: " + str);

			return ml;
		}

		private static void Notify(string msg) {
			File.AppendAllText(NOTIFY_PATH, DateTime.Now.ToBinary() + "|" + msg.Replace("\r\n", " ") + "\r\n");
		}

		public static Notify[] GetNotifies() {
			if (!File.Exists(NOTIFY_PATH)) return new Notify[0];
			var lines = File.ReadAllLines(NOTIFY_PATH);
			var notifies = new Notify[lines.Length];
			for (var i = 0; i < lines.Length; i++) {
				var str = lines[i].Split('|');
				notifies[i] = new Notify { DateTime = DateTime.FromBinary(Int64.Parse(str[0])), Message = str[1] };
			}
			return notifies;
		}

		public static void ClearNotifies() {
			File.Delete(NOTIFY_PATH);
		}

		public bool DownloadData(int fromIndex, int numRecord, TimeSpan? shift = null) {
			open_log();
			try {
				var crono = Stopwatch.StartNew();
				open_station();
				var recList = read_records(fromIndex, numRecord);
				var elapsed = crono.Elapsed;
				crono.Reset();
				Log("Tempo impiegato a leggere " + recList.Count + " record: " + elapsed.ToString(@"m\:ss\.fff"));
				if (recList.Count == 0) return true;

				// Converte gli HistoryRecord in Measure, effettuando una prima verifica sui dati
				var mList = get_measurements(recList);

				if (shift != null && !shift.Equals(TimeSpan.Zero)) {
					foreach (var m in mList) m.DateTime = m.DateTime.Add((TimeSpan)shift);
				}

				crono.Start();
				var db = new DaoMeasurements();
				db.InsertHistoryRecords(mList);
				Config.LastMem = recList.Last();
				Config.LastDt = mList.Last().DateTime;

				// Resetta contatore memoria
				var newValue = _wscom.GetStoredHistoryCount() - mList.Count;
				reset_counter(newValue);

				// Aggiorna statistiche
				update_stats();
			}
			catch (Exception ex) {
				Log("ERRORE: " + ex.Message + " Stacktrace: " + ex.StackTrace, LogLevel.Error);
				return false;
			}
			finally {
				close_station();
				close_log();
			}
			return true;
		}

		private void open_station() {
			if (_wscom.IsOpen) return;
			if (!_wscom.Open()) throw new Exception("Impossibile connettersi alla stazione meteo.");
		}

		private void close_station() {
			if (_wscom.IsOpen) _wscom.Close();
		}

		private void reconnect_station() {
			close_station();
			Thread.Sleep(1000);
			open_station();
		}

		private void reset_counter(int newValue) {
			var reset = _wscom.SetStoredHistoryCount(newValue);
			if (reset) return;
			Log("Reset del contatore a " + newValue + " non riuscito. Tentativo di riconnessione...", LogLevel.Warning);
			reconnect_station();
			reset = _wscom.SetStoredHistoryCount(newValue);
			Log("Nuovo tentativo " + (reset ? " riuscito." : "fallito."));
		}

		private List<HistoryRecord> read_new_records(int fromIndex) {
			try {
				return _wscom.GetHistoryRecords(fromIndex);
			}
			catch (IOException) {
				Log("Rilevato errore di lettura. Nuovo tentativo...", LogLevel.Warning);
				reconnect_station();
				return _wscom.GetHistoryRecords(fromIndex);
			}
		}

		private List<HistoryRecord> read_records(int fromIndex, int numRecord) {
			try {
				return _wscom.GetHistoryRecords(fromIndex, numRecord);
			}
			catch (IOException) {
				Log("Rilevato errore di lettura. Nuovo tentativo...", LogLevel.Warning);
				reconnect_station();
				return _wscom.GetHistoryRecords(fromIndex, numRecord);
			}
		}

		private HistoryRecord read_record(int index) {
			try {
				return _wscom.GetHistoryRecord(index);
			}
			catch (IOException) {
				Log("Rilevato errore di lettura. Nuovo tentativo...", LogLevel.Warning);
				reconnect_station();
				return _wscom.GetHistoryRecord(index);
			}
		}

		/// <summary>
		/// Aggiorna le statistiche elaborando i dati disponibili nel db delle misurazioni
		/// </summary>
		private void update_stats() {
			var dao = new DaoStatistics();
			var lastAvailDate = Config.LastDt.Date; // Ultimo giorno disponibile per le misurazioni
			var currDate = dao.GetLastStoredDay().Date.AddDays(1); // Primo giorno da elaborare per le statistiche
			while (currDate < lastAvailDate) {
				Log("Aggiornamento statistiche per il giorno " + currDate.ToString("dd/MM/yyyy"), LogLevel.Debug);
				var measurements = get_day_measurements(currDate);
				dao.UpdateDailyStats(currDate, get_daily_stats(measurements));
				currDate = currDate.AddDays(1);
			}
		}

		/// <summary>
		/// Forza l'aggiornamento delle statistiche per un determinato giorno
		/// </summary>
		/// <param name="day"></param>
		public void UpdateStats(DateTime day) {
			var dao = new DaoStatistics();
			var lastStatDate = dao.GetLastStoredDay().Date; // Ultimo giorno elaborato per le statistiche
			if (day.Date > lastStatDate) throw new Exception("Data non ancora elaborata per le statistiche.");

			// Aggiorna le statistiche giornaliere e, se necessario, mensili
			var measurements = get_day_measurements(day);
			dao.ChangeDailyStats(day, get_daily_stats(measurements));

			// Aggiorna le statistiche orarie, se necessario
			var fs = dao.GetFivesStats(day);
			if (fs.IsEmpty) return;

			// Ricalcola le statistiche temporanee per tutti i giorni della cinquina e poi aggiorna la cinquina
			dao.DeleteFivesStats(fs);
			for (var d = fs.FirstDay; d <= fs.LastDay; d = d.AddDays(1)) {
				var dayMeasurements = get_day_measurements(d);
				dao.CreateTempHourStats(d, get_daily_stats(dayMeasurements));
			}
		}

		public static string GetLogFile(DateTime dt) {
			return string.IsNullOrEmpty(Config.LogFolder) ? null : Config.LogFolder + "\\" + dt.ToString("yyyy-MM") + ".log";
		}

		private void wscom_OnProgress(object sender, int percentCompleted, string currentTask) {
		    OnProgress?.Invoke(this, percentCompleted, currentTask);
		}

	    private bool open_log() {
			if (LogIsOpen) return true;
			if (LogPath == null) return false;
			var fs = File.Open(LogPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
			_logger = new StreamWriter(fs);
			_internalLoggerOpen = true;
			return true;
		}

		private void close_log() {
			_internalLoggerOpen = false;
			if (_logger == null || !_isInternalLogger) return;
			//if (_logger is StreamWriter) ((StreamWriter)_logger).BaseStream.Close();
			_logger.Close();
			_logger = null;
		}

		public void Log(string msg, LogLevel lev = LogLevel.Info) {
			if (lev <= LogLevel.Warning) Notify(msg);
			if (lev > _logLevel) return;
			var wasOpen = LogIsOpen;
			if (!wasOpen && !open_log()) return;
			_logger.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff") + " - " + msg);
			_logger.Flush();
			if (!wasOpen) close_log();
		}

		/// <summary>
		/// Restituisce uno stato che rappresenta la situazione dei record estratti rispetto alla
		/// variazione ora solare/legale.
		/// </summary>
		/// <param name="dtFirst">Data/ora del primo record estratto</param>
		/// <param name="dtLast">Data/ora dell'ultimo record estratto</param>
		/// <param name="diffMin">Differenza in minuti tra orario WS8610 e PC</param>
		/// <param name="dtDst">Data/ora di riferimento variazione (inizio ora legale o inizio ora solare)</param>
		/// <returns>Stato Dst rilevato</returns>
		private Dst rileva_stato_dst(DateTime dtFirst, DateTime dtLast, int diffMin, out DateTime dtDst) {
			var tipoDst = Dst.Allineato;
			var dst = TimeZone.CurrentTimeZone.GetDaylightChanges(dtFirst.Year);
			if (dtLast < dst.Start) {
				dtDst = TimeZone.CurrentTimeZone.GetDaylightChanges(dtFirst.Year - 1).End;
				if (!Config.DstAlign) tipoDst = Dst.LegaleSolare;
			}
			else if (dtLast < dst.End) {
				dtDst = dst.Start;
				if (dtFirst <= dst.Start || !Config.DstAlign || diffMin > 50) tipoDst = Dst.SolareLegale;
			}
			else { // dt_last >= dst.End
				dtDst = dst.End;
				if (dtFirst <= dst.End || !Config.DstAlign || diffMin < -50) tipoDst = Dst.LegaleSolare;
			}

			if (tipoDst != Dst.Allineato) {
				var msg = "Rilevata variazione orario in corso: ";
				switch (tipoDst) {
					case Dst.SolareLegale:
						msg += "passaggio da ora solare a ora legale.";
						break;
					case Dst.LegaleSolare:
						msg += "passaggio da ora legale a ora solare.";
						break;
				}
				Log(msg);
				Notify(msg);
			}
			return tipoDst;
		}

		private static List<DailySensorStats> get_daily_stats(DailyMeasurements dayMeasures) {
			var nSensors = dayMeasures.Temperature.Count;
			var dailyStats = new List<DailySensorStats>(nSensors);

			for (var s = 0; s < nSensors; s++) {
				var stats = new DailySensorStats();

				// Distribuisce le misurazioni per fasce di orario (12 fasce di 2 ore)
				var ranges = new Dictionary<int, List<Decimal>>();
				foreach (var mt in dayMeasures.Temperature[s]) {
					var r = mt.Key.Hours / 2;
					if (!ranges.ContainsKey(r)) ranges[r] = new List<decimal>();
					ranges[r].Add(mt.Value);
				}
				foreach (var rt in ranges) stats.TempH24[rt.Key] = Math.Round(rt.Value.Average(), 1);

				// Calcola le statistiche. Per considerare i dati devo avere misurazioni per almeno 18 ore
				if (dayMeasures.Temperature[s].Count > 60 * 18 / M_STEP) {
					stats.TempMax = dayMeasures.Temperature[s].Values.Max();
					stats.TempMin = dayMeasures.Temperature[s].Values.Min();
					stats.TempAvg = Math.Round(dayMeasures.Temperature[s].Values.Average(), 1);
				}
				if (dayMeasures.Humidity[s].Count > 60 * 18 / M_STEP) {
					stats.HumMax = dayMeasures.Humidity[s].Values.Max();
					stats.HumMin = dayMeasures.Humidity[s].Values.Min();
					stats.HumAvg = (int?)Math.Round(dayMeasures.Humidity[s].Values.Average(), 0);
				}
				dailyStats.Add(stats);
			}

			return dailyStats;
		}

		/// <summary>
		/// Ottiene l'elenco delle misurazioni del giorno, interpolando se mancano dei dati
		/// </summary>
		/// <param name="day">Data/ora del giorno per il quale si devono estrarre le misurazioni</param>
		/// <returns>Elenco delle misurazioni interpolate</returns>
		private static DailyMeasurements get_day_measurements(DateTime day) {
			var m = new DailyMeasurements(4);

			var daoM = new DaoMeasurements();
			var dayMeasures = daoM.GetHistoryRecords(day.Date, day.Date.AddHours(23).AddMinutes(59));
			foreach (DataRow r in dayMeasures.Rows) {
				var dt = ((DateTime)r["giorno"]).Add(((DateTime)r["minuto"]).TimeOfDay);
				for (var i = 0; i < 4; i++) {
					if (!(r["temp" + i] is DBNull)) {
						var tp = Convert.ToDecimal(r["temp" + i]);
						// Cerca il valore precedente e verifica se mancano delle misurazioni intermedie
						var diff = (m.Temperature[i].Count == 0) ? T_STEP : dt.TimeOfDay - m.Temperature[i].Last().Key;
						// Considera al massimo buchi di 45 minuti
						if (diff > T_STEP && diff < TimeSpan.FromMinutes(45)) {
							// Interpolazione lineare dei valori fino a quello corrente
							var val = m.Temperature[i].Last().Value;
							var step = (tp - val) / new Decimal(diff.TotalMinutes / M_STEP);
							for (var ts = m.Temperature[i].Last().Key.Add(T_STEP); ts < dt.TimeOfDay; ts = ts.Add(T_STEP)) {
								val += step;
								m.Temperature[i][ts] = Math.Round(val, 1);
							}
						}
						m.Temperature[i][dt.TimeOfDay] = tp;
					}
					if (!(r["umid" + i] is DBNull)) {
						var um = Convert.ToInt32(r["umid" + i]);
						// Cerca il valore precedente e verifica se mancano delle misurazioni intermedie
						var diff = (m.Humidity[i].Count == 0) ? T_STEP : dt.TimeOfDay - m.Humidity[i].Last().Key;
						// Considera al massimo buchi di 45 minuti
						if (diff > T_STEP && diff < TimeSpan.FromMinutes(45)) {
							// Interpolazione lineare dei valori fino a quello corrente
							var val = (double)m.Humidity[i].Last().Value;
							var step = (um - val) / diff.TotalMinutes / M_STEP;
							for (var ts = m.Humidity[i].Last().Key.Add(T_STEP); ts < dt.TimeOfDay; ts = ts.Add(T_STEP)) {
								val += step;
								m.Humidity[i][ts] = (int)Math.Round(val, 0);
							}
						}
						m.Humidity[i][dt.TimeOfDay] = um;
					}
				}
			}
			return m;
		}
	}
}