using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using WS8610Recorder;

namespace WS8610GUI
{
	public partial class EliminaMisure : Form
	{
		public EliminaMisure() {
			InitializeComponent();
		}

		private void btOK_Click(object sender, EventArgs e) {
			btOK.Enabled = false;
			var dao = new DaoMeasurements();
			dao.DeleteHistoryRecords(dtDa.Value, dtA.Value);
			Close();
		}

		private void btAnnulla_Click(object sender, EventArgs e) {
			Close();
		}
	}
}