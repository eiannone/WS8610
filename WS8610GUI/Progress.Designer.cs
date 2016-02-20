namespace WS8610GUI
{
	partial class Progress
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
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.lbPerc = new System.Windows.Forms.Label();
			this.bgAggiorna = new System.ComponentModel.BackgroundWorker();
			this.lbTask = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(16, 28);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(205, 20);
			this.progressBar.Step = 5;
			this.progressBar.TabIndex = 24;
			// 
			// lbPerc
			// 
			this.lbPerc.AutoSize = true;
			this.lbPerc.Location = new System.Drawing.Point(227, 32);
			this.lbPerc.Name = "lbPerc";
			this.lbPerc.Size = new System.Drawing.Size(21, 13);
			this.lbPerc.TabIndex = 25;
			this.lbPerc.Text = "0%";
			// 
			// bgAggiorna
			// 
			this.bgAggiorna.WorkerReportsProgress = true;
			this.bgAggiorna.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgAggiorna_DoWork);
			this.bgAggiorna.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgAggiorna_ProgressChanged);
			this.bgAggiorna.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgAggiorna_RunWorkerCompleted);
			// 
			// lbTask
			// 
			this.lbTask.AutoSize = true;
			this.lbTask.Location = new System.Drawing.Point(13, 9);
			this.lbTask.Name = "lbTask";
			this.lbTask.Size = new System.Drawing.Size(70, 13);
			this.lbTask.TabIndex = 28;
			this.lbTask.Text = "Operazione...";
			// 
			// Database
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(266, 64);
			this.Controls.Add(this.lbTask);
			this.Controls.Add(this.lbPerc);
			this.Controls.Add(this.progressBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "Database";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Operazione";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label lbPerc;
		private System.ComponentModel.BackgroundWorker bgAggiorna;
		private System.Windows.Forms.Label lbTask;
	}
}