namespace WS8610GUI
{
	partial class ServiceStartStop
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.bgwService = new System.ComponentModel.BackgroundWorker();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.btAnnulla = new System.Windows.Forms.Button();
			this.lbDescr = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// bgwService
			// 
			this.bgwService.WorkerSupportsCancellation = true;
			this.bgwService.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwService_DoWork);
			this.bgwService.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwService_RunWorkerCompleted);
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(13, 30);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(166, 20);
			this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progressBar.TabIndex = 0;
			this.progressBar.UseWaitCursor = true;
			// 
			// btAnnulla
			// 
			this.btAnnulla.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btAnnulla.Location = new System.Drawing.Point(185, 30);
			this.btAnnulla.Name = "btAnnulla";
			this.btAnnulla.Size = new System.Drawing.Size(60, 20);
			this.btAnnulla.TabIndex = 1;
			this.btAnnulla.Text = "Annulla";
			this.btAnnulla.UseVisualStyleBackColor = true;
			this.btAnnulla.UseWaitCursor = true;
			this.btAnnulla.Click += new System.EventHandler(this.btAnnulla_Click);
			// 
			// lbDescr
			// 
			this.lbDescr.AutoSize = true;
			this.lbDescr.Location = new System.Drawing.Point(13, 11);
			this.lbDescr.Name = "lbDescr";
			this.lbDescr.Size = new System.Drawing.Size(110, 13);
			this.lbDescr.TabIndex = 2;
			this.lbDescr.Text = "Operazione in corso...";
			this.lbDescr.UseWaitCursor = true;
			// 
			// ServiceStartStop
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btAnnulla;
			this.ClientSize = new System.Drawing.Size(259, 65);
			this.ControlBox = false;
			this.Controls.Add(this.lbDescr);
			this.Controls.Add(this.btAnnulla);
			this.Controls.Add(this.progressBar);
			this.Name = "ServiceStartStop";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Controllo servizio WS8610";
			this.UseWaitCursor = true;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.ComponentModel.BackgroundWorker bgwService;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button btAnnulla;
		private System.Windows.Forms.Label lbDescr;
	}
}