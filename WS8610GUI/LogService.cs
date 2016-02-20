using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WS8610Recorder;

namespace WS8610GUI
{
	public partial class LogService : Form
	{
		public LogService() {
			InitializeComponent();
			lbPath.Text = Recorder.LogPath;
			if (lbPath.Text == null || !File.Exists(Recorder.LogPath)) {
				lbPath.Text = Recorder.GetLogFile(DateTime.Now.AddMonths(-1));
			}
			button1_Click(null, null);
		}

		private void button1_Click(object sender, EventArgs e) {
			var fs = File.Open(lbPath.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			var tr = new StreamReader(fs);
			tbLog.Text = tr.ReadToEnd();
			tr.Close();
			fs.Close();
			tbLog.Select(tbLog.Text.Length - 1, 0);
			tbLog.ScrollToCaret();
		}

		private void LogService_Shown(object sender, EventArgs e) {
			tbLog.Select(tbLog.Text.Length - 1, 0);
			tbLog.ScrollToCaret();
		}
	}
}
