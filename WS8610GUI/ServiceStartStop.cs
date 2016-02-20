using System;
using System.ComponentModel;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace WS8610GUI
{
	public partial class ServiceStartStop : Form
	{
		public ServiceStartStop(ServiceController ws_service, ServiceControllerStatus status) {
			InitializeComponent();
			bgwService.RunWorkerAsync(new object[] {ws_service, status});
			lbDescr.Text = status == ServiceControllerStatus.Running ? "Avvio in corso..." : "Arresto in corso...";
		}

		private void bgwService_DoWork(object sender, DoWorkEventArgs e) {
			var ws_service = (ServiceController) ((object[]) e.Argument)[0];
			var target_status = (ServiceControllerStatus) ((object[]) e.Argument)[1];
			if (target_status == ServiceControllerStatus.Running) ws_service.Start();
			else ws_service.Stop();

			while(ws_service.Status != target_status && !bgwService.CancellationPending) {
				Thread.Sleep(1000);
				ws_service.Refresh();
			}
		}

		private void bgwService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			Close();
		}

		private void btAnnulla_Click(object sender, EventArgs e) {
			bgwService.CancelAsync();
			btAnnulla.Enabled = false;
		}
	}
}
