using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using WS8610;

namespace WS8610Recorder
{
	public class Config
	{
        public static string ConfigDbPath { get; } = get_config_db_path();

        // Oppure: Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + @"\config.db3"
        static readonly FileSystemWatcher ConfWatcher = get_config_watcher();
		static readonly SQLiteConnection Conn = new SQLiteConnection("Data Source=" + ConfigDbPath);		
		static Dictionary<string, object> _dic = load_configuration();

		public static string DbFolder {
			get { return _dic["DbFolder"].ToString(); }
			set {
				if (value.Equals(_dic["DbFolder"])) return;
				update_value("DbFolder", value, DbType.String);
			}
		}

		public static DateTime LastDt {
			get { return (DateTime) _dic["LastDt"]; }
			set {
				if (value.Equals(_dic["LastDt"])) return;
				update_value("LastDt", value, DbType.DateTime);
			}
		}

		public static bool DstAlign {
			get { return (bool)_dic["DstAlign"]; }
			set {
				if (value.Equals(_dic["DstAlign"])) return;
				update_value("DstAlign", value, DbType.Boolean);
			}
		}

		public static string LogFolder {
			get { return _dic["LogFolder"].ToString(); }
			set {
				if (value.Equals(_dic["LogFolder"])) return;
				update_value("LogFolder", value, DbType.String);
			}
		}

		public static LogLevel LogLevel {
			get {
				LogLevel value;
				return Enum.TryParse(_dic["LogLevel"].ToString(), true, out value) ? value : LogLevel.Disable;
			}
			set {
				var strVal = value.ToString();
				if (strVal == _dic["LogLevel"].ToString()) return;
				update_value("LogLevel", strVal, DbType.String);
			}
		}

		public static long UpdateInterval {
			get { return (long)_dic["UpdateInterval"]; }
			set {
				if (value.Equals(_dic["UpdateInterval"])) return;
				update_value("UpdateInterval", value, DbType.Int64);
			}			
		}

		public static string ComPort {
			get { return _dic["ComPort"].ToString(); }
			set {
				if (value.Equals(_dic["ComPort"])) return;
				update_value("ComPort", value, DbType.String);
			}
		}

	    public static HistoryRecord LastMem {
			get {
				return new HistoryRecord(Convert.ToInt32(_dic["LastMemId"])) { DateTime = (DateTime)_dic["LastMemDt"] };
			}
			set {
				if (!value.Index.Equals(_dic["LastMemId"])) update_value("LastMemId", value.Index, DbType.Int64);
				if (!value.DateTime.Equals(_dic["LastMemDt"])) update_value("LastMemDt", value.DateTime, DbType.DateTime);
			}
		}

		static string get_config_db_path() {
			var c = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\WS8610Recorder.conf");
			var dbPath = c[0].Split('=')[1].Trim();
			if (!File.Exists(dbPath)) throw new Exception("Impossibile trovare il database config '" + dbPath + "'");
			return dbPath;
		}

		static Dictionary<string, object> load_configuration() {
			var d = new Dictionary<string, object>();
			Conn.Open();
			var cmd = new SQLiteCommand("SELECT * FROM config", Conn);
			var r = cmd.ExecuteReader();
			if (!r.HasRows) throw new Exception("Impostazioni di configurazione non trovate!");
			while(r.Read()) {
				object val;
				switch (r["type"].ToString()) {
					case "B":
						val = (bool) r["val_bool"];
						break;
					case "D":
						val = (DateTime) r["val_dt"];
						break;
					case "I":
						val = (long) r["val_int"];
						break;
					//case "S":
					default:
						val = r["val_str"];
						break;
				}
				d.Add(r["name"].ToString(), val);
			}
			r.Close();
			Conn.Close();

			return d;
		}

		static void update_value(string name, object value, DbType type) {
			_dic[name] = value;
			string colName;
			switch (type) {
				case DbType.Int32:
				case DbType.Int16:
				case DbType.Int64:
					colName = "val_int";
					break;
				case DbType.Boolean:
					colName = "val_bool";
					break;
				case DbType.DateTime:
					colName = "val_dt";
					break;
				default:
					colName = "val_str";
					break;
			}
			var cmd = new SQLiteCommand("UPDATE config SET " + colName + " = @value WHERE name = '" + name + "'", Conn);
			cmd.Parameters.Add("@value", type).Value = value;
			ConfWatcher.EnableRaisingEvents = false;
			Conn.Open();
			cmd.ExecuteNonQuery();
			Conn.Close();
			ConfWatcher.EnableRaisingEvents = true;
		}

		private static FileSystemWatcher get_config_watcher() {
			var configDir = Path.GetDirectoryName(ConfigDbPath);
			var configDb = Path.GetFileName(ConfigDbPath);
			if (configDir == null || configDb == null) throw new Exception("Percordo del db di configurazione non valido.");
			var configWatcher = new FileSystemWatcher {
				Path = configDir,
				Filter = configDb,
				EnableRaisingEvents = true,
				IncludeSubdirectories = false,
				NotifyFilter = NotifyFilters.LastWrite
			};
			configWatcher.Changed += config_db_changed;
			return configWatcher;
		}

		static void config_db_changed(object sender, FileSystemEventArgs e) => _dic = load_configuration();
	}
}
