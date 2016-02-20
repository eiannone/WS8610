using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WS8610GUI
{
	public partial class Log : Form
	{
		public LogWriter Logger;

		public Log() {
			InitializeComponent();
			Logger = new LogWriter(tbLog);
		}

		public void WriteLine(string value) {
			Logger.WriteLine(value);
		}

		private void Log_Shown(object sender, EventArgs e) {
			tbLog.DeselectAll();
		}

		private void Log_FormClosing(object sender, FormClosingEventArgs e) {
			if (e.CloseReason != CloseReason.UserClosing) return;
			e.Cancel = true;
			Hide();
		}
	}

	public class LogWriter : TextWriter
	{
		private readonly TextBox _lb;
		public LogWriter(TextBox lb) {
			_lb = lb;
		}

		public override Encoding Encoding {
			get { return Encoding.Default; }
		}

		public override void Write(string value) {
			_lb.Text += value;
		}

		public override void WriteLine(string value) {
			_lb.Text += DateTime.Now.ToString("HH:mm.ff") + @" - " + value + Environment.NewLine;
		}
	}
}
