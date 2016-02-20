using System.Windows.Forms;
using WS8610Recorder;

namespace WS8610GUI
{
	public partial class ImpostazioniDb : Form
	{
		public ImpostazioniDb() {
			InitializeComponent();

			tbConfigDb.Text = Config.ConfigDbPath;
			tbDbDati.Text = Config.DbFolder;
			tbLog.Text = Config.LogFolder;
			tbLogLevel.Text = Config.LogLevel.ToString();
			tbPortaCom.Text = Config.ComPort;
			tbIntAgg.Text = Config.UpdateInterval + @" h";
			tbDtUlt.Text = Config.LastDt.ToString("dd/MM/yyyy HH:mm");
		}

		private void ImpostazioniDb_Shown(object sender, System.EventArgs e) {
			tbConfigDb.DeselectAll();
		}
	}
}
