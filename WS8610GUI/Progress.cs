using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WS8610GUI
{
	public delegate void VoidDelegate();

	public partial class Progress : Form
	{
		public VoidDelegate DoWork;
		public VoidDelegate OnWorkCompleted;

		public Progress() {
			InitializeComponent();
		}

		public void RunAsync() {
			bgAggiorna.RunWorkerAsync();
		}

		private void bgAggiorna_DoWork(object sender, DoWorkEventArgs e) {
			if (DoWork != null) DoWork();
		}

		public void UpdateProgress(object sender, int percent_completed, string current_task) {
			bgAggiorna.ReportProgress(percent_completed, current_task);
		}

		private void bgAggiorna_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			Invoke((MethodInvoker) delegate {
				progressBar.Value = Math.Min(e.ProgressPercentage, 100);
				lbPerc.Text = progressBar.Value + @"%";
				lbTask.Text = e.UserState.ToString();
			});
		}

		private void bgAggiorna_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			lbTask.Text = @"Aggiornamento completato.";
			if (OnWorkCompleted != null) OnWorkCompleted();
		}
	}
}
