namespace WS8610GUI
{
	partial class Log
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
			this.tbLog = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tbLog
			// 
			this.tbLog.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbLog.Location = new System.Drawing.Point(0, 0);
			this.tbLog.Multiline = true;
			this.tbLog.Name = "tbLog";
			this.tbLog.ReadOnly = true;
			this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbLog.Size = new System.Drawing.Size(707, 172);
			this.tbLog.TabIndex = 0;
			this.tbLog.WordWrap = false;
			// 
			// Log
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(707, 172);
			this.Controls.Add(this.tbLog);
			this.MaximizeBox = false;
			this.Name = "Log";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Log";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Log_FormClosing);
			this.Shown += new System.EventHandler(this.Log_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbLog;
	}
}