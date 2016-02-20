using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WS8610Recorder;

namespace WS8610GUI
{
	public partial class TrayNotify : Form
	{
		public TrayNotify(IEnumerable<Notify> notifies) {
			InitializeComponent();
			var bottom = 0;
			foreach(var n in notifies) {
				var orario = ((n.DateTime.Date == DateTime.Now.Date) ? "" : n.DateTime.ToString("dd/MM ")) + n.DateTime.ToString("HH:mm");
				var r = dgLog.Rows.Add(orario, n.Message);
				bottom += dgLog.Rows[r].Height;
			}
			if (bottom > dgLog.Height) dgLog.Columns[1].Width = 245;
		}

		private void TrayNotify_Load(object sender, EventArgs e) {
			Left = Screen.PrimaryScreen.WorkingArea.Width - Width - 10;
			Top = Screen.PrimaryScreen.WorkingArea.Height - Height - 10;
			Opacity = 0;
			timer.Start();
		}

		private void timer_Tick(object sender, EventArgs e) {
			if (Opacity < 1) Opacity += 0.08;
			else timer.Stop();
		}

		private void btChiudi_Click(object sender, EventArgs e) {
			Close();
			Recorder.ClearNotifies();			
		}

		private void lkLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Close();
			Recorder.ClearNotifies();
			var w = new LogService();
			w.Show();
		}
	}
}
