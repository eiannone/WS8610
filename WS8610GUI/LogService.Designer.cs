namespace WS8610GUI
{
	partial class LogService
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
            this.components = new System.ComponentModel.Container();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.lbPath = new System.Windows.Forms.Label();
            this.btAggiorna = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // tbLog
            // 
            this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLog.BackColor = System.Drawing.Color.White;
            this.tbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbLog.Location = new System.Drawing.Point(0, 24);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(654, 398);
            this.tbLog.TabIndex = 0;
            this.tbLog.WordWrap = false;
            // 
            // lbPath
            // 
            this.lbPath.AutoSize = true;
            this.lbPath.Location = new System.Drawing.Point(2, 5);
            this.lbPath.Name = "lbPath";
            this.lbPath.Size = new System.Drawing.Size(35, 13);
            this.lbPath.TabIndex = 1;
            this.lbPath.Text = "label1";
            this.toolTip.SetToolTip(this.lbPath, "Percorso file di log");
            // 
            // btAggiorna
            // 
            this.btAggiorna.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btAggiorna.Image = global::WS8610GUI.Properties.Resources.arrow_refresh;
            this.btAggiorna.Location = new System.Drawing.Point(623, 1);
            this.btAggiorna.Name = "btAggiorna";
            this.btAggiorna.Size = new System.Drawing.Size(26, 22);
            this.btAggiorna.TabIndex = 2;
            this.toolTip.SetToolTip(this.btAggiorna, "Aggiorna log");
            this.btAggiorna.UseVisualStyleBackColor = true;
            this.btAggiorna.Click += new System.EventHandler(this.button1_Click);
            // 
            // LogService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 422);
            this.Controls.Add(this.btAggiorna);
            this.Controls.Add(this.lbPath);
            this.Controls.Add(this.tbLog);
            this.MaximizeBox = false;
            this.Name = "LogService";
            this.ShowIcon = false;
            this.Text = "Log";
            this.Shown += new System.EventHandler(this.LogService_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbLog;
		private System.Windows.Forms.Label lbPath;
		private System.Windows.Forms.Button btAggiorna;
		private System.Windows.Forms.ToolTip toolTip;
	}
}