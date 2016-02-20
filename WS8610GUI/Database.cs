using System;
using System.IO;
using System.Windows.Forms;
using WS8610Recorder;

namespace WS8610GUI
{
	public partial class Database : Form
	{
		private readonly Recorder rec;
		private bool success;

		public Database(TextWriter logger = null) {
			InitializeComponent();
			rec = new Recorder(logger);
		}

		private void Database_Load(object sender, EventArgs e) {
			update_last_rec_date();
		}

		private void btAggiorna_Click(object sender, EventArgs e) {
			update_last_rec_date();
			btAggiorna.Enabled = false;
			var progress = new Progress { Text = @"Aggiornamento database" };
			rec.OnProgress += progress.UpdateProgress;
			progress.DoWork = delegate() {
				if (rbTutti.Checked) success = rec.DownloadNewData(true);
				else {
					var shift = TimeSpan.Zero;
					if (cbOffsetG.Checked && tbGiorni.Text.Length > 0) {
						var gg = int.Parse(tbGiorni.Text);
						shift = TimeSpan.FromDays(gg);
					}
					if (cbOffset.Checked) {
						var ts = new TimeSpan(int.Parse(tbOffset.Text.Substring(1, 2)), int.Parse(tbOffset.Text.Substring(4, 2)), 0);
						shift = tbOffset.Text.StartsWith("-") ? shift.Subtract(ts) : shift.Add(ts);
					}
					var da = int.Parse(tbDa.Text);
					var a = int.Parse(tbA.Text);
					if (da <= a) success = rec.DownloadData(da, a - da + 1, shift);
				}
			};
			progress.OnWorkCompleted = delegate() {
				rec.OnProgress -= progress.UpdateProgress;
				progress.Close();
				update_last_rec_date();
				btAggiorna.Enabled = true;
				var msg = success ? "Aggiornamento completato con successo" : "Aggiornamento fallito. Controllare log.";
				MessageBox.Show(msg, "", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
			};
			progress.RunAsync();
			progress.ShowDialog(this);
		}

		private void update_last_rec_date() {
			var dt = rec.GetLastRecordDateTime();
			lbData.Text = dt.ToString("dd/MM/yyyy");
			lbOra.Text = dt.ToString("HH:mm");
			var dt_mem = Config.LastMem.DateTime;
			lbMemDt.Text = dt_mem.ToString("dd/MM/yyyy");
			lbMemOra.Text = dt_mem.ToString("HH:mm");
			if (lbData.Text == lbMemDt.Text && lbOra.Text == lbMemOra.Text) {
				lbMem.Enabled = lbMem2.Enabled = lbMemDt.Enabled = lbMemOra.Enabled = false;
			}
		}

		private void tbDa_Enter(object sender, EventArgs e) {
			rbDaA.Checked = true;
			tbOffset.Enabled = cbOffset.Enabled = true;
			tbGiorni.Enabled = cbOffsetG.Enabled = true;
		}

		private void rbTutti_CheckedChanged(object sender, EventArgs e) {
			tbGiorni.Enabled = cbOffsetG.Enabled = tbOffset.Enabled = cbOffset.Enabled = !rbTutti.Checked;
		}
	}
}