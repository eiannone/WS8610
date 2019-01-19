using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace WS8610
{
	public delegate void ProgressEventHandler(object sender, int percentCompleted, string currentTask);

	public class WS8610Com
	{
		public const short HISTORY_BASE_ADDR = 0x0064;                           // Starting address where history data is stored
		public const short HISTORY_BUFFER_SIZE = 0x7FFF - HISTORY_BASE_ADDR + 1; // Size of history buffer, in bytes (32.668)
		public const int LOG_LEVEL = 1;
		private const int MICRODELAY = 4;                                        // microseconds wait for microdelay method (decrease for more speed)
		private const int OPENING_TIMEOUT_SECS = 10;
		private const int MAX_READ_RETRIES = 10;
		private readonly Stopwatch _delayer = new Stopwatch();
		private readonly TextWriter _logger;
		public static readonly double MicrodelayTicks = get_microdelay_ticks();

		public event ProgressEventHandler OnProgress;

		private readonly WS8610SerialWrapper _sp;
		private int _extSensors = -1;
		private int _historyRecSize = -1;

		public bool IsOpen => _sp.Status == PortStatus.Open;

	    /// <summary>
		/// Number of external sensors recorded
		/// </summary>
		public int ExtSensors {
			get {
			    if (_extSensors != -1) return _extSensors;
			    var wasClose = !IsOpen;
			    if (wasClose && !Open()) return _extSensors;
			    var data = read_safe(0x0C, 1);
			    _extSensors = data.Length == 0 ? -1 : data[0] & 0x0F;
			    if (wasClose) Close();
			    return _extSensors;
			}
		}

		/// <summary>
		/// Size in bytes of a single history record
		/// </summary>
		public int HistoryRecSize {
			get {
			    if (_historyRecSize != -1) return _historyRecSize;
			    switch (ExtSensors) {
			        case 2:
			            _historyRecSize = 13;
			            break;
			        case 3:
			            _historyRecSize = 15;
			            break;
			        default:
			            _historyRecSize = 10;
			            break;
			    }
			    return _historyRecSize;
			}
		}

		/// <summary>
		/// Max number of available records in history buffer.
		/// (3.266 for 0 or 1 external sensor, 2.512 for 2 and 2.177 for 3 sensors, which correspond
		/// respectively to 11d 8h and 50', 8d 17h and 20', 7d 13h 25', considering 288 records a day)
		/// </summary>
		public int MaxHistoryRecords => HISTORY_BUFFER_SIZE / HistoryRecSize;

	    public WS8610Com(string portname, TextWriter logger = null) {
			_logger = logger ?? Console.Out;
			_sp = new WS8610SerialWrapper(portname);
		}

		public bool Open() {
			print_log(1, "open_weatherstation");
			if (!_sp.Open()) throw new Exception("Unable to open port.");
			set_DTR(false);
			set_RTS(false);
			int i;

			// Wait for DSR signal (Data Set Ready)
			const int retryDelayMs = 20;
			const int maxRetries = OPENING_TIMEOUT_SECS * 1000 / retryDelayMs;
			for (i = 0; i < maxRetries; i++) {
				Thread.Sleep(retryDelayMs);
				if (get_DSR()) break;
			}
			if (i == maxRetries) {
				print_log(2, "Connection timeout 1");
				Close();
				return false;
			}

			// Wait for DSR stop
			for (i = 0; i < maxRetries; i++) {
				Thread.Sleep(retryDelayMs);
				if (!get_DSR()) break;
			}
		    if (i < maxRetries) return true;
		    print_log(2, "Connection timeout 2");
		    Close();
		    return false;
		}

		public void Close() {
			_sp.Close();
		}

		/// <summary>
		/// Get number of history records stored in memory
		/// </summary>
		/// <returns>Number of history records stored in memory</returns>
		public int GetStoredHistoryCount() {
			var b = read_safe(0x0009, 2);
			return (b[1] >> 4) * 1000 + (b[1] & 0x0F) * 100 + (b[0] >> 4) * 10 + (b[0] & 0x0F);
		}

		/// <summary>
		/// Reset 'mem' indicator to 0000.
		/// </summary>
		/// <returns>true if success</returns>
		public bool ResetStoredHistoryCount() {
			return SetStoredHistoryCount(0);
		}

		/// <summary>
		/// Set 'mem' indicator to arbitrary value.
		/// </summary>
		/// <param name="value">Value to set</param>
		/// <returns>true if success</returns>
		public bool SetStoredHistoryCount(int value) {
			var ones = (byte)(value % 10);
			var tens = (byte)((value % 100) / 10);
			var hundreds = (byte)((value % 1000) / 100);
			var thousands = (byte)((value % 10000) / 1000);
			return WriteMemory(0x0009, new[] { (byte)((tens << 4) | ones), (byte)((thousands << 4) | hundreds) });
		}

		/// <summary>
		/// Returns last available history record.
		/// </summary>
		/// <returns>Last available history record</returns>
		public HistoryRecord GetLastHistoryRecord() {
			var rec0 = GetHistoryRecord(0);
			if (!rec0.IsValid) throw new Exception("No data available");

			// Try to guess position of last record
			int lastRecNum;
			var dtRec0 = rec0.DateTime;
			var dtLast = get_last_record_datetime();
			if (dtRec0 > dtLast) {
				// Inconsistent dates, so scan memory and find last record manually
				lastRecNum = get_num_rec_behind_marker() - 1;
			}
			else {
				lastRecNum = Convert.ToInt16((dtLast - dtRec0).TotalMinutes / 5) % MaxHistoryRecords;
				var lastRec = GetHistoryRecord(lastRecNum);
				// Check guessed last record, if invalid scans memory and find it manually
				if (lastRec.IsValid && lastRec.DateTime == dtLast) {
					return lastRec.IsLast ? lastRec : GetHistoryRecord(lastRecNum + 1);
				}
				lastRecNum = get_num_rec_behind_marker() - 1;
			}
			return GetHistoryRecord(lastRecNum);
		}

		/// <summary>
		/// Returns last available history records.
		/// </summary>
		/// <param name="numRec">Number of records to return</param>
		/// <returns>Last available history records</returns>
		public List<HistoryRecord> GetLastHistoryRecords(int numRec) {
			var rec0 = GetHistoryRecord(0);
			if (!rec0.IsValid) throw new Exception("No data available");

			// Try to guess position of last record
			int lastRecNum;
			var dtRec0 = rec0.DateTime;
			var dtLast = get_last_record_datetime();
			if (dtRec0 > dtLast) {
				// Inconsistent dates, so scan memory and find last record manually
				lastRecNum = get_num_rec_behind_marker() - 1;
			}
			else {
				lastRecNum = Convert.ToInt16((dtLast - dtRec0).TotalMinutes / 5) % MaxHistoryRecords;
				var lastRec = GetHistoryRecord(lastRecNum);
				// Check guessed last record, if invalid scans memory and find it manually
				if (lastRec.IsValid && lastRec.DateTime == dtLast) {
					if (!lastRec.IsLast) lastRecNum++;
				}
				else {
					lastRecNum = get_num_rec_behind_marker() - 1;
				}
			}

			// Check rolled data
			if (lastRecNum >= numRec - 1) {
				return GetHistoryRecords(lastRecNum - numRec + 1, numRec);
			}
			else {
				// Scan also into rolled data
				var rolled = Math.Min(numRec, MaxHistoryRecords) - lastRecNum - 1;
				var hrList = GetHistoryRecords(MaxHistoryRecords - rolled, rolled);
				hrList.AddRange(GetHistoryRecords(0, lastRecNum + 1));
				return hrList;
			}
		}

		/// <summary>
		/// Dump a range of addresses of WS8610 memory
		/// </summary>
		/// <param name="startAddr">Starting address (0 - 0x7FFF)</param>
		/// <param name="numBytes">Number of bytes to extract</param>
		/// <param name="report">True to report progress (default = false)</param>
		/// <returns>Array of bytes representing memory locations</returns>
		public byte[] DumpMemory(short startAddr, int numBytes, bool report = false) {
			var endAddr = startAddr + numBytes - 1;
			if (startAddr < 0 || endAddr > 0x7FFF) {
				throw new Exception("Invalid address range: " + startAddr.ToString("X4") + " - " + endAddr.ToString("X4"));
			}
			if (report) report_progress("Dumping memory", 0);
			if (numBytes <= 2000) return read_safe(startAddr, numBytes);
			var mem = new byte[numBytes];
			var pos = 0;
			while (pos < numBytes) {
				if (report) {
					var percent = Math.Min(100, pos * 100 / numBytes);
					report_progress("Dumping memory: " + pos + " of " + numBytes + " bytes read", percent);
				}
				var bufSize = Math.Min(2000, numBytes - pos);
				var buf = read_safe((short)(startAddr + pos), bufSize);
				buf.CopyTo(mem, pos);
				pos += bufSize;
			}
			if (report) report_progress("Dumping memory", 100);
			return mem;
		}

		/// <summary>
		/// Write some bytes to WS8610 memory
		/// </summary>
		/// <param name="startAddr">Starting address (0 - 0x7FFF)</param>
		/// <param name="bytesToWrite">bytes to write</param>
		/// <returns>True if success</returns>
		public bool WriteMemory(short startAddr, byte[] bytesToWrite) {
			start_sequence();
			if (!query_address(startAddr)) return false;
			if (bytesToWrite.Any(b => !send_byte(b))) return false;
			end_command();

			start_sequence();
			for (var i = 0; i < 3; i++) send_command(0xA0, false);

			set_DTR(false);
			Microdelay();
			get_CTS();
			set_DTR(true);
			Microdelay();

			return compare_bytes(DumpMemory(startAddr, bytesToWrite.Length), bytesToWrite);
		}

        /// <summary>
        /// Return a list of history records starting from an index.
        /// Records are returned until "recordsToExtract" count is reached, or the last record is found,
        /// or last memory address is reached, or an invalid record is found (invalid record is not returned).
        /// </summary>
        /// <param name="startingRec">Starting index of records (0 based)</param>
        /// <param name="recordsToExtract">Maximum records to extract</param>
        /// <returns>List of records</returns>
        public List<HistoryRecord> GetHistoryRecords(int startingRec, int recordsToExtract) {
			var recList = new List<HistoryRecord>(recordsToExtract);
			if (recordsToExtract == 0) return recList;
			if (startingRec < 0 || startingRec >= MaxHistoryRecords) throw new Exception("Record " + startingRec + " out of range.");

			var startingAddr = (short)(HISTORY_BASE_ADDR + (startingRec * HistoryRecSize));
			var blockSize = Math.Min((recordsToExtract * HistoryRecSize) + 1, 0x7FFF - startingAddr + 1);
			var memDump = DumpMemory(startingAddr, blockSize);
			// Convert bytes into HistoryRecords
			var recBytes = new byte[HistoryRecSize + 1];
			var index = startingRec;
			for (var i = 0; i < blockSize - HistoryRecSize; i += HistoryRecSize) {
				Array.Copy(memDump, i, recBytes, 0, recBytes.Length);
				var hr = decode_history_record(recBytes, index++);
				if (hr.IsValid) recList.Add(hr);
				if (!hr.IsValid || hr.IsLast) break;
			}
			return recList;
		}

		/// <summary>
		/// Read a history record from memory. Always return a record, which can eventually be invalid
		/// </summary>
		/// <param name="recordNo">index of record in memoery</param>
		/// <returns>A history record, which can eventually have IsValid = false</returns>
		public HistoryRecord GetHistoryRecord(int recordNo) {
			var recs = GetHistoryRecords(recordNo, 1);
			return (recs.Count == 1) ? recs[0] : new HistoryRecord(recordNo);
		}

		/// <summary>
		/// Returns all valid history records stored in memory
		/// </summary>
		/// <param name="report">True to report progress (default = false)</param>
		/// <returns>All history records stored in WS8610 memory</returns>
		public List<HistoryRecord> GetAllHistoryRecords(bool report = false) {
			if (report) report_progress("Reading all history records", 0);
			var recList = new List<HistoryRecord>(MaxHistoryRecords);
			if (has_rolled()) {
				// Dumps entire history memory and converts it into history records
				var memDump = DumpMemory(HISTORY_BASE_ADDR, HISTORY_BUFFER_SIZE, report);
				int i;
				var recBytes = new byte[HistoryRecSize + 1];

				// Converts first part, until "last record" marker
				if (report) report_progress("Decoding data 1/2", 94);
				var index = 0;
				for (i = 0; i < memDump.Length - HistoryRecSize; i += HistoryRecSize) {
					Array.Copy(memDump, i, recBytes, 0, recBytes.Length);
					var hr = decode_history_record(recBytes, index++);
					if (hr.IsValid) recList.Add(hr);
					if (hr.IsLast) break;
				}

				// Check for rolled records and eventually converts them
				if (report) report_progress("Decoding data 2/2", 96);
				i += HistoryRecSize;
				if (i + (2 * HistoryRecSize) < memDump.Length) {
					// Decodes the "rolled" record and try to reconstruct it based on the next one
					Array.Copy(memDump, i, recBytes, 0, recBytes.Length);
					recBytes[0] = 0; // index 0 is the minutes, set it to fake 0
					var hrRolled = decode_history_record(recBytes, index++);

					// Decodes the first "not rolled" record
					i += HistoryRecSize;
					Array.Copy(memDump, i, recBytes, 0, recBytes.Length);
					var hrNotRolled = decode_history_record(recBytes, index++);

					if (hrRolled.IsValid && hrNotRolled.IsValid) {
						// Try to set the minute part of the rolled record 5 minutes before the not rolled one
						var guessMin = (hrNotRolled.DateTime.Minute == 0) ? 55 : hrNotRolled.DateTime.Minute - 5;
						hrRolled.DateTime = hrRolled.DateTime.AddMinutes(guessMin);

						// Compares the two records, if the difference is 5 minutes then consider also the rolled one
						if ((hrNotRolled.DateTime - hrRolled.DateTime).TotalMinutes == 5) recList.Add(hrRolled);
					}

					if (hrNotRolled.IsValid) recList.Add(hrNotRolled);

					// Adds all remaining records, until end of memory buffer
					i += HistoryRecSize;
					while (i < memDump.Length - HistoryRecSize) {
						Array.Copy(memDump, i, recBytes, 0, recBytes.Length);
						var hr = decode_history_record(recBytes, index++);
						if (hr.IsValid) recList.Add(hr);
						i += HistoryRecSize;
					}
				}
			}
			else {
				// Scans memory in blocks of 100 records, starting from record 0, until "last record" marker is found
				var blStart = 0;
				bool reachedLast;
				do {
					if (report) {
						var percent = blStart * 100 / MaxHistoryRecords;
						report_progress("Reading all history records: " + blStart + " of " + MaxHistoryRecords, percent);
					}
					var blRec = GetHistoryRecords(blStart, 100);
					recList.AddRange(blRec);
					reachedLast = (blRec.Count != 100 || blRec[blRec.Count - 1].IsLast);
					blStart += 100;
				} while (!reachedLast);
			}
			if (report) report_progress("Reading all history records", 100);
			return recList;
		}

		/// <summary>
		/// Return a list of history records, recorded after a specified date/time (not included)
		/// </summary>
		/// <param name="fromDate">Date/time to start searching from</param>
		/// <param name="report">True to report progress (default = false)</param>
		/// <returns>List of history records recorded after the specified date</returns>
		public List<HistoryRecord> GetHistoryRecords(DateTime fromDate, bool report = false) {
			var recList = new List<HistoryRecord>(MaxHistoryRecords);
			if (report) report_progress("Reading records", 0);
			var rec0 = GetHistoryRecord(0);
			var dtLast = get_last_record_datetime();
			if (!rec0.IsValid || fromDate >= dtLast.AddMinutes(5)) return recList;

			// CONSISTENCE CHECK
			// Try to determine if memory data is consistent (i.e. consecutive records with max 1 hour of missing
			// data), so we can skip directly to requested record, otherwise we need to extract all records from
			// memory and check them all.
			if (report) report_progress("Consistence check", 1);
			var consistent = false;
			var readRolled = false;
			// Estimate last record index, based on date/time difference with record 0
			short blStart = 0;
			var estimatedRecords = Convert.ToInt16((dtLast - rec0.DateTime).TotalMinutes / 5);
			var lastRecIndex = estimatedRecords;
			// Allow a 50 record margin, to compensate for daylight saving (± 12 records)
			// and erroneous missing records (2% of 2.000 is 40 records).
			if (lastRecIndex < MaxHistoryRecords + 50) {
				// Estimate starting record index, based on date/time difference with record 0 or last record
				if (fromDate >= rec0.DateTime) {
					// If starting date is after record 0 we will search for the starting record between 0 and the
					// "last record" marker.
					//
					// Estimate starting record index, based on date/time difference with record 0
					blStart = Convert.ToInt16((fromDate - rec0.DateTime).TotalMinutes / 5);
					estimatedRecords -= blStart;
					if (blStart < lastRecIndex + 50) {
						// Extract a block of 100 records centered on estimated one (± 50 records)
						blStart = (short)Math.Min(Math.Max(1, blStart - 50), MaxHistoryRecords - 1);
						consistent = true;
					}
				}
				else if (has_rolled()) {
					// Else if starting date is before record 0, we will search for the start index between last_record
					// and end of memory.
					//
					var recMax = GetHistoryRecord(MaxHistoryRecords - 1);
					// Estimate starting record index, based on date/time difference with record rec_max
					blStart = (short)(MaxHistoryRecords - Convert.ToInt16((recMax.DateTime - fromDate).TotalMinutes / 5));
					estimatedRecords += (short)(MaxHistoryRecords - blStart);
					if (blStart > lastRecIndex - 50) {
						// Extract a block of 100 records centered on estimated one (± 50 records)
						blStart = (short)Math.Min(Math.Max(1, blStart - 50), MaxHistoryRecords - 1);
						consistent = true;
						readRolled = true;
					}
				}
				else {
					// Else if starting date is before record 0 and history has not rolled, we assume that data is consistent
					// because we will fetch anyway all (valid) records from 0 to last
					consistent = true;
				}
			}

			var recRead = 0;
			if (consistent) {
				if (report) report_progress("Reading records: 0 of " + estimatedRecords, 3);
				var recBl = GetHistoryRecords(blStart, 100);
				if (recBl.Count > 0 && recBl[0].DateTime <= fromDate && recBl[recBl.Count - 1].DateTime >= fromDate) {
					// Copies valid records to result
					recList.AddRange(recBl.Where(r => r.DateTime > fromDate));
					blStart += (short)recBl.Count;
				}
			}

			if (recList.Count == 0) {
				// Estimate failed, probably data is not consistent, so extract all records from memory and check them all
				var allRec = GetAllHistoryRecords(report);
				recList.AddRange(allRec.Where(r => r.DateTime > fromDate));
				return recList;
			}

			// OK, DATA SEEMS CONSISTENT, SO SKIP DIRECTLY TO REQUESTED RECORDS
			if (!recList[recList.Count - 1].IsLast) {
				// Continue extracting blocks of 100 records until "last record" marker is found
				List<HistoryRecord> recBl;
				do {
					if (report) {
						recRead += 100;
						var percent = recRead * 100 / estimatedRecords;
						report_progress("Reading records: " + recRead + " of " + estimatedRecords, percent);
					}
					recBl = GetHistoryRecords(blStart, 100);
					recList.AddRange(recBl.Where(r => r.DateTime > fromDate));
					blStart += 100;
				} while (recBl.Count == 100 && !recBl[recBl.Count - 1].IsLast);
			}
			if (readRolled) {
				// If read_rolled is set to true, previous loop has read only rolled records, so now read also not rolled
				List<HistoryRecord> recBl;
				blStart = 0;
				do {
					if (report) {
						recRead += 100;
						var percent = recRead * 100 / estimatedRecords;
						report_progress("Reading records: " + recRead + " of " + estimatedRecords, percent);
					}
					recBl = GetHistoryRecords(blStart, 100);
					recList.AddRange(recBl.Where(r => r.DateTime > fromDate));
					blStart += 100;
				} while (recBl.Count == 100 && !recBl[recBl.Count - 1].IsLast);
			}
			if (report) report_progress("Reading records", 100);
			return recList;
		}

		/// <summary>
		/// Return a list of all history records available, starting from a specified record id.
		/// Detect if history rolled and continue extracting from beginning.
		/// </summary>
		/// <param name="fromRec">Record id to start extracting from (0 based)</param>
		/// <returns>List of all history records starting from specified record id until last record available</returns>
		public List<HistoryRecord> GetHistoryRecords(int fromRec) {
			if (fromRec < 0) return GetAllHistoryRecords();

			var maxHr = MaxHistoryRecords;
			var recList = new List<HistoryRecord>(maxHr);
			var rec0 = GetHistoryRecord(0);
			if (!rec0.IsValid || fromRec >= maxHr) return recList;

			var blStart = fromRec;
			// Read records until last record found or last memory address reached
			do {
				var recBl = GetHistoryRecords(blStart, Math.Min(100, maxHr - blStart));
				recList.AddRange(recBl);
				if (recBl.Count == 0 || recBl.Last().IsLast) break;
				blStart += recBl.Count;
			} while (blStart < maxHr);

			// Check if needs to continue from address 0 (history rolled)
		    if (recList.Count == 0 || recList.Last().Index != maxHr - 1) return recList;

		    // History rolled, continue extracting from record 0
		    blStart = 0;
		    do {
		        var recBl = GetHistoryRecords(blStart, Math.Min(100, maxHr - blStart));
		        recList.AddRange(recBl);
		        if (recBl.Count == 0 || recBl.Last().IsLast) break;
		        blStart += recBl.Count;
		    } while (blStart < fromRec);

		    if (recList.Last().IsLast) return recList;

		    // If last record is not found, then the last record is the one at MaxHistoryRecords - 1 index
		    // So remove all records between record 0 and from_rec
		    recList.RemoveRange(recList.Count - blStart, blStart);

		    return recList;
		}

        /// <summary>
        /// Set temperature value fo a history record in memory.
        /// </summary>
        /// <param name="recordNr">Index of record in memory (0 based)</param>
        /// <param name="sensorNr">Sensor number (0 = base station)</param>
        /// <param name="temp">Temperature value</param>
        /// 
        public bool SetTemp(int recordNr, int sensorNr, double temp)
	    {
            if (sensorNr > ExtSensors) throw new Exception("Requested sensor not available");
            var offsets = new[] { 5, 6, 10, 12 };
            var startingAddr = (short) (HISTORY_BASE_ADDR + (recordNr * HistoryRecSize) + offsets[sensorNr]);
            var b = DumpMemory(startingAddr, 2);
            switch (sensorNr) {
                case 0:
                case 2:
                    b[0] = (byte) ((((temp*10))%10) + (((byte) Math.Floor(temp)%10) << 4));
                    b[1] = (byte) ((b[1] & 0xF0) + Math.Floor(temp/10) + 3);
                    break;
                case 1:
                case 3:
                    b[0] = (byte) (((byte) (((temp*10))%10) << 4) + (b[0] & 0x0F));
                    b[1] = (byte) (((byte) Math.Floor(temp)%10) + ((byte) (Math.Floor(temp/10) + 3) << 4));
                    break;
            }
            return WriteMemory(startingAddr, b);
	    }

        /// <summary>
        /// Set humidity value fo a history record in memory.
        /// Each set is 10 bytes for one sensor, 13 bytes for two sensors or 15 bytes for three sernsors
        /// bytes 1-2: Time, bytes 3-5: Date, 
        /// bytes 6-8: Temp0 + Temp1, byte 9: RH0, byte 10 RH1
        /// bytes 11-13: Temp2, RH2
        /// bytes 13-15: Temp3, RH3
        /// </summary>
        /// <param name="recordNr">Index of record in memory (0 based)</param>
        /// <param name="sensorNr">Sensor number (0 = base station)</param>
        /// <param name="humidity">Humidity value</param>
        /// 
        public bool SetHumidity(int recordNr, int sensorNr, int humidity)
        {
            if (sensorNr > ExtSensors) throw new Exception("Requested sensor not available");
            var offsets = new[] { 8, 9, 11, 14 };
            var startingAddr = (short)(HISTORY_BASE_ADDR + (recordNr * HistoryRecSize) + offsets[sensorNr]);
            var b = DumpMemory(startingAddr, 2);
            switch (sensorNr)
            {
                case 0:
                case 1:
                case 3:
                    b[0] = (byte)(((humidity / 10) << 4) + (humidity % 10));
                    break;
                case 2:
                    b[0] = (byte)(((byte)(humidity % 10) << 4) | (b[0] & 0x0F));
                    b[1] = (byte)((humidity / 10) + (b[1] & 0xF0));
                    break;
            }
            return WriteMemory(startingAddr, b);
        }

        /// <summary>
        /// Delete one recorded data for one sensor
        /// </summary>
        /// <param name="recordNr">Index of record in memory (0 based)</param>
        /// <param name="sensorNr">Sensor number (0 = base station)</param>
        /// <returns></returns>
	    public bool DeleteRecord(int recordNr, int sensorNr) {
            if (sensorNr > ExtSensors) throw new Exception("Requested sensor not available");
            var offset = (sensorNr < 2)? 5 : 10;
            var startingAddr = (short)(HISTORY_BASE_ADDR + (recordNr * HistoryRecSize) + offset);
            var b = DumpMemory(startingAddr, 5);
            switch (sensorNr) {
                case 0:
                    b[0] = 0xAA;
                    b[1] = (byte)((b[1] & 0xF0) | 0x0A);
                    b[3] = 0xAA;
                    break;
                case 1:
                    b[1] = (byte)((b[1] & 0x0F) | 0xA0);
                    b[2] = 0xAA;
                    b[4] = 0xAA;
                    break;
                case 2:
                    b[0] = 0xAA;
                    b[1] = 0xAA;
                    b[2] = (byte)((b[2] & 0xF0) | 0x0A);
                    break;
                case 3:
                    b[2] = (byte)((b[2] & 0x0F) | 0xA0);
                    b[3] = 0xAA;
                    b[4] = 0xAA;
                    break;
            }
            return WriteMemory(startingAddr, b);
        }

		#region private methods

		private void report_progress(string action, int percent) {
		    OnProgress?.Invoke(this, percent, action);
		}

	    private void set_DTR(bool dtr) {
			print_log(5, dtr ? "Set DTR" : "Clear DTR");
			_sp.SetDTR(dtr);
		}

		private void set_RTS(bool rts) {
			print_log(5, rts ? "Set RTS" : "Clear RTS");
			_sp.SetRTS(rts);
		}

		private bool get_DSR() {
			var dsr = _sp.GetDSR();
			print_log(5, "Got DSR = " + (dsr ? "1" : "0"));
			return dsr;
		}

		private bool get_CTS() {
			var cts = _sp.GetCTS();
			print_log(5, "Got CTS = " + (cts ? "1" : "0"));
			return cts;
		}

		/// <summary>
		/// Converts raw bytes of memory into a HistoryRecord object
		/// </summary>
		/// <param name="b">Bytes to convert</param>
		/// <param name="index">History record index</param>
		/// <returns>History record</returns>
		private HistoryRecord decode_history_record(IList<byte> b, int index = 0) {
			var min  = (b[0] >> 4) * 10 + (b[0] & 0x0F);
			var hour = (b[1] >> 4) * 10 + (b[1] & 0x0F);
			var mday = (b[2] >> 4) * 10 + (b[2] & 0x0F);
			var mon  = (b[3] >> 4) * 10 + (b[3] & 0x0F);
			var year = (b[4] >> 4) * 10 + (b[4] & 0x0F) + 2000;
			var hr = new HistoryRecord(index);
			try {
				hr.DateTime = new DateTime(year, mon, mday, hour, min, 0);
			}
			catch (ArgumentException) {
				return hr;
			}
			hr.IsValid = true;
			hr.Temp = new double[ExtSensors + 1];
			hr.Hum = new int[ExtSensors + 1];
			hr.Temp[0] = ((b[6] & 0x0F) * 10 + (b[5] >> 4) + (b[5] & 0x0F) / 10.0) - 30.0;
			hr.Hum[0] = (b[8] >> 4) * 10 + (b[8] & 0xF);
			switch (ExtSensors) {
				case 1:
					hr.Temp[1] = ((b[7] & 0x0F) + (b[7] >> 4) * 10 + (b[6] >> 4) / 10.0) - 30.0;
					hr.Hum[1] = (b[9] >> 4) * 10 + (b[9] & 0x0F);
					break;
				case 2:
					hr.Temp[2] = ((b[11] & 0x0F) * 10 + (b[10] >> 4) + (b[10] & 0x0F) / 10.0) - 30.0;
					hr.Hum[2] = (b[11] >> 4) + (b[12] & 0x0F) * 10;
					goto case 1;
				case 3:
					hr.Temp[3] = ((b[13] & 0x0F) + (b[13] >> 4) * 10 + (b[12] >> 4) / 10.0) - 30.0;
					hr.Hum[3] = (b[14] >> 4) * 10 + (b[14] & 0x0F);
					goto case 2;               
			}

			hr.IsLast = (b[HistoryRecSize] == 0xFF);
			return hr;
		}

		/// <summary>
		/// Date and time of last (or last but one) recording
		/// This date/time seems to be updated every 10 minutes, not 5
		/// </summary>
		/// <returns>Date and time of last (or last but one) recording</returns>
		private DateTime get_last_record_datetime() {
			var dt = read_safe(0x0000, 6);
			if (dt.Length != 6) {
				Close();
				throw new Exception("Cannot obtain last recording date/time");
			}

			var min = ((dt[0] >> 4) * 10) + (dt[0] & 0x0F);
			var hour = ((dt[1] >> 4) * 10) + (dt[1] & 0x0F);
			var day = (dt[2] >> 4) + ((dt[3] & 0x0F) * 10);
			var month = (dt[3] >> 4) + ((dt[4] & 0x0F) * 10);
			var year = 2000 + (dt[4] >> 4) + ((dt[5] & 0xF) * 10);

			return new DateTime(year, month, day, hour, min, 0);
		}

		/// <summary>
		/// </summary>
		/// <returns>Number of records behind "last record" marker</returns>
		private int get_num_rec_behind_marker() {
			var pos = HISTORY_BASE_ADDR;
			var step = (short)HistoryRecSize;
			byte[] b;
			var records = 0;
			do {
				records++;
				pos += step;
				b = read_data(pos, 1);
			} while (b[0] != 0xFF);

			return records;
		}

		private byte[] read_safe(short address, int bytesToRead) {
			print_log(1, "read_safe");
			var buf = new byte[bytesToRead];
			int j;
			for (j = 0; j < MAX_READ_RETRIES; j++) {
				start_sequence();
				buf = read_data(address, bytesToRead);
				start_sequence();
				var readdata2 = read_data(address, bytesToRead);
				if (buf.Length == 0 || !compare_bytes(buf, readdata2)) {
					print_log(2, "read_safe - two readings not identical");
					continue;
				}
				// Check if only 0's for reading memory range greater than 10 bytes
				print_log(2, "read_safe - two readings identical");
				var i = 0;
				if (bytesToRead > 10) for (; buf[i] == 0 && i < bytesToRead; i++) { }
				if (i != bytesToRead) break;
				print_log(2, "read_safe - only zeros");
			}
			// If we have tried MAX_READ_RETRIES times to read we expect not to have valid data
			if (j == MAX_READ_RETRIES) throw new IOException("Read failed (tried " + MAX_READ_RETRIES + " times)");

			return buf;
		}

		private static bool compare_bytes(ICollection<byte> a, IList<byte> b) {
			if (a.Count != b.Count) return false;
			return !a.Where((t, i) => t != b[i]).Any();
		}

		private byte[] read_data(short address, int bytesToRead) {
			if (!query_address(address) || !send_command(0xA1)) return new byte[] { };
			var readdata = new byte[bytesToRead];
			readdata[0] = read_byte();
			for (var i = 1; i < bytesToRead; i++) {
				cmd_read_next();
				readdata[i] = read_byte();
			}
			end_command();

			return readdata;
		}

		private bool query_address(short address) {
			return send_command(0xA0) && send_byte((byte)(address / 256)) && send_byte((byte)(address % 256));
		}

		private bool send_command(byte cmd, bool checkvalue = true) {
			set_DTR(false);
			Microdelay();
			set_RTS(false);
			Microdelay();
			set_RTS(true);
			Microdelay();
			set_DTR(true);
			Microdelay();
			set_RTS(false);
			Microdelay();

			return send_byte(cmd, checkvalue);
		}

		private void start_sequence() {
			print_log(3, "start_sequence");
			set_RTS(false);
			Microdelay();
			set_DTR(false);
			Microdelay();
		}

		private void end_command() {
			print_log(3, "end_command");
			set_RTS(true);
			Microdelay();
			set_DTR(false);
			Microdelay();
			set_RTS(false);
			Microdelay();
		}

		private void cmd_read_next() {
			print_log(3, "read_next_byte_seq");
			set_RTS(true);
			Microdelay();
			set_DTR(false);
			Microdelay();
			set_DTR(true);
			Microdelay();
			set_RTS(false);
			Microdelay();
		}

		private bool send_byte(byte b, bool checkvalue = true) {
			print_log(3, "Send byte 0x" + b.ToString("X2"));

			for (var i = 0; i < 8; i++) {
				write_bit((b & 0x80) > 0);
				b <<= 1;
			}
			set_RTS(false);
			Microdelay();
		    if (!checkvalue) return true;

		    var status = get_CTS();
		    //TODO: checking value of status, error routine
		    Microdelay();
		    set_DTR(false);
		    Microdelay();
		    set_DTR(true);
		    Microdelay();
		    return status;
		}

		private byte read_byte() {
			print_log(3, "Read byte ...");
			byte b = 0;
			for (var i = 0; i < 8; i++) {
				b *= 2;
				b += read_bit();
			}
			print_log(3, "Byte = 0x" + b.ToString("X2"));
			return b;
		}

		private void write_bit(bool bit) {
			print_log(4, "Write bit " + (bit ? "1" : "0"));
			set_RTS(!bit);
			Microdelay();
			set_DTR(false);
			Microdelay();
			set_DTR(true);
		}

		private byte read_bit() {
			print_log(4, "Read bit ...");
			set_DTR(false);
			Microdelay();
			var status = get_CTS();
			Microdelay();
			set_DTR(true);
			Microdelay();
			print_log(4, "Bit = " + (status ? "0" : "1"));

			return (byte)(status ? 0 : 1);
		}

		/// <summary>
		/// Check if history has rolled (i.e. records beyond last, until end of memory, are valid)
		/// </summary>
		/// <returns>True if rolled</returns>
		private bool has_rolled() {
			var b = read_data(0x000B, 2);
			return b[0] != 0; // TODO: Check rolling detection routine!
		}

		private void print_log(int level, string txt) {
			if (level < LOG_LEVEL) _logger.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff") + " - " + txt);
		}

		private static double get_microdelay_ticks() {
			if (!Stopwatch.IsHighResolution) throw new Exception("Stopwatch not available in high resolution");
			return Stopwatch.Frequency * MICRODELAY / 1e6;
		}

		private void Microdelay() {
			_delayer.Start();
			while (_delayer.ElapsedTicks < MicrodelayTicks) Thread.SpinWait(50);
			_delayer.Reset();
		}

		#endregion private methods
	}
}