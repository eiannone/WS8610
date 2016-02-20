using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;

namespace WS8610Recorder
{
	public class DaoStatistics
	{
		private static readonly CultureInfo N_FORMAT = CultureInfo.CreateSpecificCulture("en-US");
		readonly SQLiteConnection conn;

		public DaoStatistics() {
			var db_path = Config.DbFolder + @"\Statistiche.db3";
			conn = new SQLiteConnection(String.Format("Data Source={0}", db_path));
		}

		public DateTime GetLastStoredDay() {
			const string sql = "SELECT data FROM stat_giorni " +
												 "ORDER BY data DESC " +
												 "LIMIT 0, 1";
			var cmd = new SQLiteCommand(sql, conn);
			var was_closed = open_connection();
			var dr = cmd.ExecuteReader();
			if (!dr.Read()) throw new Exception("Nessun dato disponibile nel db.");
			var dt = dr.GetDateTime(0);
			dr.Close();
			if (was_closed) close_connection();
			return dt;
		}

		public void UpdateDailyStats(DateTime day, List<DailySensorStats> stats) {
			var was_closed = open_connection();
			var transaction = conn.BeginTransaction();			
			try {
				// Inserimento statistiche giornaliere
				var sql = "INSERT INTO stat_giorni VALUES('" + day.ToString("yyyy-MM-dd") + "'";
				for (var s = 0; s < 4; s++) {
					sql += ", " + sql_dec(stats[s].TempMax) + ", " + sql_dec(stats[s].TempMin) + ", " + sql_dec(stats[s].TempAvg);
				}
				for (var s = 0; s < 4; s++) {
					sql += ", " + sql_dec(stats[s].HumMax) + ", " + sql_dec(stats[s].HumMin) + ", " + sql_dec(stats[s].HumAvg);
				}
				sql += ")";
				var cmd_giorni = new SQLiteCommand(sql, conn, transaction);
				cmd_giorni.ExecuteNonQuery();

				// Se è l'ultimo giorno del mese aggiorna anche le statistiche mensili
				if (day.AddDays(1).Month != day.Month) update_monthly_stats(day, transaction);

				// Inserimento statistiche giornaliere per fasce orarie
				CreateTempHourStats(day, stats, transaction);

				transaction.Commit();
				if (was_closed) close_connection();
			}
			catch (Exception) {
				transaction.Rollback();
				close_connection();
				throw;
			}
		}

		public void CreateTempHourStats(DateTime day, IList<DailySensorStats> stats, SQLiteTransaction transaction = null) {
			var was_closed = open_connection();
			var cinquina = update_temph_stats(day, stats, transaction);

			// Se è l'ultimo giorno di una cinquina aggiorna anche le statistiche orarie per cinquine
			var g_anno = day.DayOfYear - 1;
			var ult_g_anno = (new DateTime(day.Year, 12, 31)).DayOfYear - 1;
			if ((g_anno % 5 != 4 || g_anno > 363) && g_anno != ult_g_anno) {
				if (was_closed) close_connection();
				return;
			}

			var sql_orari = "INSERT INTO stat_ore SELECT " + day.Year + ", sensore, cinquina";
			for (var f = 0; f < 24; f += 2) sql_orari += ", round(avg(" + String.Format("h{0:D2}_{1:D2}", f, f + 2) + "), 1)";
			sql_orari += " FROM stat_ore_temp WHERE cinquina = " + cinquina + " GROUP BY sensore, cinquina";
			var cmd_orari = new SQLiteCommand(sql_orari, conn, transaction);
			cmd_orari.ExecuteNonQuery();

			var cmd_orari2 = new SQLiteCommand("DELETE FROM stat_ore_temp WHERE cinquina = " + cinquina, conn, transaction);
			cmd_orari2.ExecuteNonQuery();
			if (was_closed) close_connection();
		}

		private int update_temph_stats(DateTime day, IList<DailySensorStats> stats, SQLiteTransaction transaction) {
			var cinquina = Math.Min((day.DayOfYear - 1) / 5, 72);
			var cmd_ore = new SQLiteCommand("INSERT INTO stat_ore_temp VALUES('" + day.ToString("yyyy-MM-dd") + "', @sens, " + cinquina,
				conn, transaction);
			cmd_ore.Parameters.Add("@sens", DbType.Int16);
			for (var r = 0; r < 12; r++) {
				cmd_ore.CommandText += ", @f" + r;
				cmd_ore.Parameters.Add("@f" + r, DbType.Decimal);
			}
			cmd_ore.CommandText += ")";
			for (var s = 0; s < stats.Count; s++) {
				if (stats[s].IsEmpty) continue;
				cmd_ore.Parameters["@sens"].Value = s;
				for (var r = 0; r < 12; r++) cmd_ore.Parameters["@f" + r].Value = stats[s].TempH24[r] ?? (object)DBNull.Value;
				cmd_ore.ExecuteNonQuery();
			}
			return cinquina;
		}

		public void ChangeDailyStats(DateTime day, List<DailySensorStats> stats) {
			var was_closed = open_connection();
			var transaction = conn.BeginTransaction();
			try {
				// Aggiornamento statistiche giornaliere
				var values = "";
				for (var s = 0; s < 4; s++) {
					values += ", t" + s + "_max = " + sql_dec(stats[s].TempMax) +
						", t" + s + "_min = " + sql_dec(stats[s].TempMin) +
						", t" + s + "_max = " + sql_dec(stats[s].TempAvg);
				}
				for (var s = 0; s < 4; s++) {
					values += ", u" + s + "_max = " + sql_dec(stats[s].HumMax) +
						", u" + s + "_min = " + sql_dec(stats[s].HumMin) +
						", u" + s + "_avg = " + sql_dec(stats[s].HumAvg);
				}
				var sql = "UPDATE stat_giorni SET " + values.Substring(2) + " WHERE data = '" + day.ToString("yyyy-MM-dd") + "'";
				var cmd_giorni = new SQLiteCommand(sql, conn, transaction);
				cmd_giorni.ExecuteNonQuery();

				// Verifica ed eventualmente aggiorna anche le statistiche mensili
				var cmd_del_mese = new SQLiteCommand("DELETE FROM stat_mesi WHERE anno = " + day.Year + " AND mese = " + day.Month, 
					conn, transaction);
				if (cmd_del_mese.ExecuteNonQuery() == 1) update_monthly_stats(day, transaction);

				// Verifica ed eventualmente aggiorna anche le statistiche orarie temporanee
				var cmd_del_ore = new SQLiteCommand("DELETE FROM stat_ore_temp WHERE data = '" + day.ToString("yyyy-MM-dd") + "'",
					conn, transaction);
				if (cmd_del_ore.ExecuteNonQuery() > 0) update_temph_stats(day, stats, transaction);

				transaction.Commit();
				if (was_closed) close_connection();
			}
			catch (Exception) {
				transaction.Rollback();
				close_connection();
				throw;
			}
		}

		private void update_monthly_stats(DateTime day, SQLiteTransaction transaction) {
			var sql_mesi = "INSERT INTO stat_mesi SELECT " + day.Year + ", " + day.Month;
			for (var s = 0; s < 4; s++) sql_mesi += ", max(t" + s + "_max), min(t" + s + "_min), round(avg(t" + s + "_avg), 1)";
			for (var s = 0; s < 4; s++) sql_mesi += ", max(u" + s + "_max), min(u" + s + "_min), round(avg(u" + s + "_avg))";
			sql_mesi += " FROM stat_giorni WHERE substr(data, 1, 7) = '" + day.ToString("yyyy-MM") + "'";
			var cmd_mesi = new SQLiteCommand(sql_mesi, conn, transaction);
			cmd_mesi.ExecuteNonQuery();			
		}

		public FivesStats GetFivesStats(DateTime day) {
			var cinquina = Math.Min((day.DayOfYear - 1) / 5, 72);
			var sql = "SELECT * FROM stat_ore WHERE anno = " + day.Year + " AND cinquina = " + cinquina + " ORDER BY sensore";
			var was_closed = open_connection();
			var dr = new SQLiteDataAdapter(sql, conn);
			var t = new DataTable();
			dr.Fill(t);
			if (was_closed) close_connection();

			var fs = new FivesStats(day.Year, cinquina) {IsEmpty = (t.Rows.Count == 0)};
			foreach (DataRow row in t.Rows) {
				var s = (byte)row["sensore"];
				for(var h = 0; h < 24; h+=2) {
					var col = String.Format("h{0:D2}_{1:D2}", h, h + 2);
					if (!(row[col] is DBNull)) fs.TempH24[s][h/2] = (decimal) row[col];
				}
			}
			return fs;
		}

		/// <summary>
		/// Open db connection
		/// </summary>
		/// <returns>True if was closed</returns>
		private bool open_connection() {
			if (conn.State != ConnectionState.Open) {
				conn.Open();
				return true;
			}
			return false;
		}
		private void close_connection() {
			conn.Close();
		}

		static string sql_dec(decimal? num) {
			return (num != null)? ((decimal)num).ToString(N_FORMAT) : "NULL";
		}

		public void DeleteFivesStats(FivesStats fs) {
			var was_closed = open_connection();
			var cmd_del = new SQLiteCommand("DELETE FROM stat_ore WHERE anno = " + fs.Year + " AND cinquina = " + fs.Index, conn);
			cmd_del.ExecuteNonQuery();
			if (was_closed) close_connection();
		}
	}
}
