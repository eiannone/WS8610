using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using WS8610Recorder;

namespace WS8610Service
{
	public partial class WS8610Service : ServiceBase
	{
		public const string SERVICE_NAME = "WS8610 Service";
		public const int CMD_LOG_CONFIG = 200;
		private readonly Recorder rec;
		private bool updating;

		public WS8610Service() {
			InitializeComponent();
			ServiceName = SERVICE_NAME;
            rec = new Recorder();
		}

		protected override void OnStart(string[] args) {
			rec.Log("Servizio avviato.");
			check_and_update();
		}

		protected override void OnPause() {
			rec.Log("Servizio in pausa.");
			// Verifica se ci sono download in corso e attende che vengano completati
			wait_if_updating(60);
			base.OnPause();
		}

		protected override void OnStop() {
			rec.Log("Servizio arrestato.");
			// Verifica se ci sono download in corso e attende che vengano completati
			wait_if_updating(60);
			base.OnStop();
		}

		protected override void OnShutdown() {
			rec.Log("Arresto del sistema in corso... servizio arrestato.");
			// Verifica se ci sono download in corso e attende che vengano completati
			wait_if_updating(60);
			base.OnShutdown();
		}

		protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus) {
			if (powerStatus == PowerBroadcastStatus.QuerySuspend) {
				// Richiesta sospensione PC in corso, verifica se ci sono download in corso e attende che vengano completati
				wait_if_updating(60);
			}
			var ret = base.OnPowerEvent(powerStatus);
			if (!ret) return false;

			if (powerStatus == PowerBroadcastStatus.ResumeSuspend) check_and_update();
			return true;
		}

		void wait_if_updating(int timeout_secs) {
			var timeout = timeout_secs / 2;
			while (updating && timeout-- > 0) {
				RequestAdditionalTime(30000);
				Thread.Sleep(2000);
			}			
		}

		void check_and_update() {
			const string msg = "Controllo aggiornamento... ";
			var dt = rec.GetLastRecordDateTime();
			if (dt.AddHours(Config.UpdateInterval) > DateTime.Now) {
				var prev = dt.AddHours(Config.UpdateInterval) - DateTime.Now;
				var hours = (int) prev.TotalHours;
				var str_prev = (hours == 0)
												? prev.Minutes + " minut" + ((prev.Minutes == 1) ? "o" : "i")
												: hours + " or" + ((hours == 1) ? "a" : "e");
				rec.Log(msg + "non necessario (previsto tra " + str_prev + ").");
				return;
			}
			rec.Log(msg + "necessario.");

			// Se la CPU è sotto carico aspetta che scende sotto il 70%
			var timeout = 360; // Aspetta al massimo un'ora
			var pf = new PerformanceCounter("Processor", "% Processor Time", "_Total");
			pf.NextValue();
			Thread.Sleep(1000);
			var cpu = (int)pf.NextValue();
			rec.Log("CPU: " + cpu + "%");
			while (timeout-- > 0 && (cpu > 60 || aggiornamento_in_corso())) {
				Thread.Sleep(10000);
				cpu = (int)pf.NextValue();
				rec.Log("CPU: " + cpu + "%");
			}

			// Scarica i dati
			updating = true;
			rec.DownloadNewData();
			updating = false;
		} 

		/// <summary>
		/// Rileva se c'è un aggiornamento imminente dei dati sulla stazione meteo (avviene ogni 5 minuti), 
		/// ovvero l'orario corrente è a cavallo di un aggiornamento 
		/// </summary>
		/// <returns>true se siamo nel minuto a cavallo di un aggiornamento</returns>
		static bool aggiornamento_in_corso() {
			var min5 = DateTime.Now.Minute % 5;
			var sec = DateTime.Now.Second;
			return (min5 == 4 && sec > 30) || (min5 == 0 && sec < 30);
		}

		protected override void OnCustomCommand(int command) {
			if (command == CMD_LOG_CONFIG) {
				rec.Log("Percorso db configurazione: " + Config.ConfigDbPath);
				rec.Log("Cartella db dati: " + Config.DbFolder);
				rec.Log("Cartella log: " + Config.LogFolder);
				rec.Log("Livello log: " + Config.LogLevel);
				rec.Log("Porta com: " + Config.ComPort);
				rec.Log("Intervallo aggiornamento: " + Config.UpdateInterval + " h");
				var dt = rec.GetLastRecordDateTime();
				rec.Log("Ultima registrazione salvata sul db: " + dt.ToString("dd/MM/yyyy HH:mm"));
			}
			base.OnCustomCommand(command);
		}
	}
}
