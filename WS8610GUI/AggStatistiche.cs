using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using WS8610Recorder;

namespace WS8610GUI
{
	public partial class AggStatistiche : Form
	{
		private readonly TextWriter _logger;

		public AggStatistiche(TextWriter logger = null) {
			InitializeComponent();
			_logger = logger;
		}

		private void btOK_Click(object sender, EventArgs e) {
			btOK.Enabled = false;
			var rec = new Recorder(_logger);
			rec.UpdateStats(dtForza.Value);
			Close();
		}

		private void btAnnulla_Click(object sender, EventArgs e) {
			Close();
		}
	}
}
