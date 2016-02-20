using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace WS8610Recorder
{
	public class DaoMeasurements
	{
		readonly SQLiteConnection conn;
		readonly string db_folder;
		private int curryear;

		public DaoMeasurements() {
			conn = new SQLiteConnection();
			db_folder = Config.DbFolder;
		}

		public Measure GetLastRecord() {
			var year = DateTime.Now.Year;
			if (!File.Exists(db_folder + "\\" + year + ".db3")) {
				year--;
				if (!File.Exists(db_folder + "\\" + year + ".db3")) throw new Exception("Nessun dato disponibile.");
			}
			set_curr_year(year, false);
			const string sql = "SELECT g.[giorno], n.[minuto], m.[int_temp] AS temp0, m.[int_umid] AS umid0, " +
					"m.[temp1], m.[umid1], m.[temp2], m.[umid2], m.[temp3], m.[umid3] " +
				"FROM misurazioni m " +
				"INNER JOIN giorni g ON g.id = m.id_giorno " +
				"INNER JOIN minuti n ON n.id = m.id_minuti " +
				"ORDER BY g.[giorno] DESC, n.[minuto] DESC " +
				"LIMIT 0, 1";
			var cmd = new SQLiteCommand(sql, conn);
			conn.Open();
			var dr = cmd.ExecuteReader();
			if (!dr.Read()) throw new Exception("Nessun dato disponibile nel db.");

			var m = new Measure(dr.GetDateTime(0).Add(dr.GetDateTime(1).TimeOfDay));
			for (var s = 0; s < 4; s++) {
				if (!(dr["temp" + s] is DBNull)) m.Sensor[s].Temp = Convert.ToDecimal(dr["temp" + s]);
				if (!(dr["umid" + s] is DBNull)) m.Sensor[s].Humid = Convert.ToInt16(dr["umid" + s]);
			}
			dr.Close();
			conn.Close();
			return m;
		}

		public Dictionary<DateTime, int[]> GetNullTemps(int sensor, DateTime from, DateTime to) {
			var t = new DataTable();
			read_null_temps(sensor, from, to, ref t);
			var nulls = new Dictionary<DateTime, int[]>(Convert.ToInt32((to - from).TotalDays));
			for (var date = from.Date; date <= to.Date; date = date.AddDays(1)) {
				nulls[date] = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			}
			foreach (DataRow dr in t.Rows) {
				var dt = (DateTime)dr["giorno"];
				var perc = (int)Math.Round((decimal)((long)dr["nulli"]) * 100 / 12);
				nulls[dt][int.Parse((string)dr["ora"])] = perc;
			}
			return nulls;
		}

		public void InsertHistoryRecords(List<Measure> m_list) {
			if (m_list.Count == 0) return;
			set_curr_year(m_list[0].DateTime.Year);
			conn.Open();
			const string sql = "INSERT INTO misurazioni VALUES(@id_g, @id_m, @t1, @u1, @t2, @u2, @t3, @u3, @t4, @u4)";
			var cmd = new SQLiteCommand(sql, conn, conn.BeginTransaction());
			cmd.Parameters.Add("@id_g", DbType.Int16);
			cmd.Parameters.Add("@id_m", DbType.Int16);
			for (var s = 1; s <= 4; s++) {
				cmd.Parameters.Add("@t" + s, DbType.Decimal);
				cmd.Parameters.Add("@u" + s, DbType.Byte);
			}
			try {
				foreach (var rec in m_list) {
					if (rec.DateTime.Year != curryear) {
						cmd.Transaction.Commit();
						conn.Close();
						set_curr_year(rec.DateTime.Year);
						conn.Open();
						cmd.Transaction = conn.BeginTransaction();
					}
					cmd.Parameters["@id_g"].Value = rec.DateTime.DayOfYear;
					cmd.Parameters["@id_m"].Value = rec.MinutesId;
					for (var s = 1; s <= 4; s++) {
						cmd.Parameters["@t" + s].Value = rec.Sensor[s - 1].Temp ?? (object)DBNull.Value;
						cmd.Parameters["@u" + s].Value = rec.Sensor[s - 1].Humid ?? (object)DBNull.Value;
					}
					cmd.ExecuteNonQuery();
				}
				cmd.Transaction.Commit();
				conn.Close();
			}
			catch (Exception ex) {
				cmd.Transaction.Rollback();
				conn.Close();
				throw new Exception("Query: '" + cmd.CommandText + "'", ex);
			}
		}

		public DataTable GetHistoryRecords(DateTime from, DateTime to) {
			var t = new DataTable();
			read_history_records(from, to, ref t);
			return t;
		}

		public void DeleteHistoryRecords(DateTime from, DateTime to) {
			if (from >= to) throw new Exception("Intervallo di date non valido.");
			if (from.Year == to.Year) {
				set_curr_year(from.Year, false);
				var g1 = from.DayOfYear;
				var m1 = Convert.ToInt32(Math.Floor(from.TimeOfDay.TotalMinutes / 5) * 2) + 1;
				var g2 = to.DayOfYear;
				var m2 = Convert.ToInt32(Math.Floor(to.TimeOfDay.TotalMinutes / 5) * 2) + 2;
				conn.Open();
				if (g1 == g2) {
					var cmd = new SQLiteCommand("DELETE FROM misurazioni " +
						"WHERE id_giorno = " + g1 + " AND id_minuti BETWEEN " + m1 + " AND " + m2, conn);
					cmd.ExecuteNonQuery();
				}
				else {
					var cmd1 = new SQLiteCommand("DELETE FROM misurazioni WHERE id_giorno = " + g1 + " AND id_minuti >= " + m1, conn);
					cmd1.ExecuteNonQuery();
					if (g2 > g1 + 1) {
						var cmd3 = new SQLiteCommand("DELETE FROM misurazioni WHERE id_giorno BETWEEN " + (g1 + 1) + " AND " + (g2 - 1), conn);
						cmd3.ExecuteNonQuery();
					}
					var cmd2 = new SQLiteCommand("DELETE FROM misurazioni WHERE id_giorno = " + g2 + " AND id_minuti <= " + m2, conn);
					cmd2.ExecuteNonQuery();
				}
				conn.Close();
			}
			else {
				var new_dt = new DateTime(from.Year, 12, 31, 23, 59, 59);
				DeleteHistoryRecords(from, new_dt);
				DeleteHistoryRecords(new_dt.AddSeconds(1), to);
			}
		}

		#region private methods

		/// <summary>
		/// Ottiene tutti i record con intervallo di date specificato (estremi imclusi) e li inserisce in un DataTable
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="t"></param>
		private void read_history_records(DateTime from, DateTime to, ref DataTable t) {
			if (from >= to) throw new Exception("Intervallo di date non valido.");
			if (from.Year == to.Year) {
				set_curr_year(from.Year, false);
				var d1 = from.ToString("yyyy-MM-dd HH:mm:ss");
				var d2 = to.ToString("yyyy-MM-dd HH:mm:ss");
				var sql = "SELECT g.[giorno], n.[minuto], m.[int_temp] AS temp0, m.[int_umid] AS umid0, " +
						"m.[temp1], m.[umid1], m.[temp2], m.[umid2], m.[temp3], m.[umid3] " +
					"FROM misurazioni m " +
					"INNER JOIN giorni g ON g.id = m.id_giorno " +
					"INNER JOIN minuti n ON n.id = m.id_minuti " +
					"WHERE datetime(g.[giorno], n.[minuto]) BETWEEN '" + d1 + "' AND '" + d2 + "' " +
					"ORDER BY g.[giorno] ASC, n.[minuto] ASC";
				var dr = new SQLiteDataAdapter(sql, conn);
				dr.Fill(t);
			}
			else {
				var new_dt = new DateTime(from.Year, 12, 31, 23, 59, 59);
				read_history_records(from, new_dt, ref t);
				read_history_records(new_dt.AddSeconds(1), to, ref t);
			}
		}

		private void read_null_temps(int sensor, DateTime from, DateTime to, ref DataTable t) {
			if (from >= to) throw new Exception("Intervallo di date non valido.");
			if (from.Year == to.Year) {
				set_curr_year(from.Year);
				var col = (sensor == 0) ? "int_temp" : "temp" + sensor;
				var d1 = from.ToString("yyyy-MM-dd");
				var d2 = to.ToString("yyyy-MM-dd");
				var sql = "SELECT g.[giorno], substr(n.[minuto], 1, 2) AS ora, COUNT(*) AS nulli " +
					"FROM misurazioni m " +
					"INNER JOIN giorni g ON g.id = m.id_giorno " +
					"INNER JOIN minuti n ON n.id = m.id_minuti " +
					"WHERE m.[" + col + "] IS NULL AND g.[giorno] BETWEEN '" + d1 + "' AND '" + d2 + "' " +
					"GROUP BY g.[giorno], ora " +
					"ORDER BY g.[giorno], n.[minuto]";
				var dr = new SQLiteDataAdapter(sql, conn);
				dr.Fill(t);
			}
			else {
				var new_dt = new DateTime(from.Year, 12, 31, 23, 59, 59);
                read_null_temps(sensor, from, new_dt, ref t);
                read_null_temps(sensor, new_dt.AddSeconds(1), to, ref t);
			}
		}

		private void create_db_for_year(int year) {
			var db_path = db_folder + "\\" + year + ".db3";
			if (File.Exists(db_path)) throw new Exception("Impossibile creare il database '" + db_path + "'. File già esistente.");
			SQLiteConnection.CreateFile(db_path);
			open_db(db_path);

			// Crea la tabella con i giorni
			var cmd = new SQLiteCommand("CREATE TABLE giorni (id INTEGER PRIMARY KEY, giorno DATE NOT NULL UNIQUE)", conn);
			cmd.ExecuteNonQuery();
			// Inserisce i giorni per l'anno indicato
			var cmd_insert = new SQLiteCommand("INSERT INTO giorni VALUES (@id, @giorno)", conn, conn.BeginTransaction());
			cmd_insert.Parameters.Add("@id", DbType.Int16);
			cmd_insert.Parameters.Add("@giorno", DbType.Date);
			var dt = new DateTime(year, 1, 1, 0, 0, 0);
			do {
				cmd_insert.Parameters["@id"].Value = dt.DayOfYear;
				cmd_insert.Parameters["@giorno"].Value = dt.Date;
				cmd_insert.ExecuteNonQuery();
				dt = dt.AddDays(1);
			} while (dt.Year == year);
			cmd_insert.Transaction.Commit();

			// Crea la tabella con i minuti
			cmd = new SQLiteCommand("CREATE TABLE minuti (id INTEGER PRIMARY KEY, minuto TIME NOT NULL, ora_legale BOOL NOT NULL)", conn);
			cmd.ExecuteNonQuery();
			var cmd_minuti = new SQLiteCommand("INSERT INTO minuti VALUES (@id, @min, @ora_leg)", conn, conn.BeginTransaction());
			cmd_minuti.Parameters.Add("@id", DbType.Int16);
			cmd_minuti.Parameters.Add("@min", DbType.String);
			cmd_minuti.Parameters.Add("@ora_leg", DbType.Boolean);
			var minuti = new TimeSpan(0, 0, 0);
			var id_minuti = 1;
			do {
				cmd_minuti.Parameters["@id"].Value = id_minuti;
				cmd_minuti.Parameters["@min"].Value = minuti.ToString();
				cmd_minuti.Parameters["@ora_leg"].Value = false;
				cmd_minuti.ExecuteNonQuery();
				cmd_minuti.Parameters["@id"].Value = id_minuti + 1;
				cmd_minuti.Parameters["@ora_leg"].Value = true;
				cmd_minuti.ExecuteNonQuery();
				minuti += TimeSpan.FromMinutes(5);
				id_minuti += 2;
			} while (minuti.TotalMinutes < 1440);
			cmd_minuti.Transaction.Commit();

			// Crea la tabella per le misurazioni
			const string sql = "CREATE TABLE misurazioni (" +
				"id_giorno SMALLINT(2) NOT NULL, " +
				"id_minuti SMALLINT(2) NOT NULL, " +
				"int_temp DECIMAL(3, 1), " +
				"int_umid TINYINT(1), " +
				"temp1 DECIMAL(3, 1), " +
				"umid1 TINYINT(1), " +
				"temp2 DECIMAL(3, 1), " +
				"umid2 TINYINT(1), " +
				"temp3 DOUBLE(3, 1), " +
				"umid3 TINYINT(1)" +
			")";
			cmd = new SQLiteCommand(sql, conn);
			cmd.ExecuteNonQuery();
			cmd = new SQLiteCommand("CREATE UNIQUE INDEX [unique time] ON misurazioni (id_giorno, id_minuti)", conn);
			cmd.ExecuteNonQuery();

			conn.Close();
		}

		private void open_db(string db_path) {
			var connection_str = String.Format("Data Source={0}", db_path);
			if (conn.ConnectionString != connection_str) {
				if (!File.Exists(db_path)) throw new Exception("Database '" + db_path + "' non esistente.");
				if (conn.State != ConnectionState.Closed) throw new Exception("Connessione già in uso.");
				conn.ConnectionString = String.Format("Data Source={0}", db_path);
			}
			if (conn.State == ConnectionState.Closed) conn.Open();
		}

		private void set_curr_year(int year, bool create = true) {
			if (year == curryear) return;
			var db_path = db_folder + "\\" + year + ".db3";
			if (!File.Exists(db_path)) {
				if (create) create_db_for_year(year);
				else throw new Exception("Database per l'anno " + year + " non disponibile.");
			}
			conn.ConnectionString = String.Format("Data Source={0}", db_path);
			curryear = year;
		}

		#endregion private methods
	}
}