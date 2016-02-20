namespace WS8610GUI
{
	partial class ShowMemory
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
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label1;
			this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.btLeggi = new System.Windows.Forms.Button();
			this.tbBytes = new System.Windows.Forms.MaskedTextBox();
			this.tbIndir = new System.Windows.Forms.MaskedTextBox();
			this.bt08 = new System.Windows.Forms.Button();
			this.bt4B = new System.Windows.Forms.Button();
			this.bt7FF0 = new System.Windows.Forms.Button();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			label2 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(110, 15);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(35, 13);
			label2.TabIndex = 9;
			label2.Text = "bytes:";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(8, 15);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(48, 13);
			label1.TabIndex = 7;
			label1.Text = "Indirizzo:";
			// 
			// flowLayoutPanel
			// 
			this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel.Location = new System.Drawing.Point(11, 41);
			this.flowLayoutPanel.Name = "flowLayoutPanel";
			this.flowLayoutPanel.Size = new System.Drawing.Size(265, 213);
			this.flowLayoutPanel.TabIndex = 12;
			// 
			// btLeggi
			// 
			this.btLeggi.Location = new System.Drawing.Point(223, 11);
			this.btLeggi.Name = "btLeggi";
			this.btLeggi.Size = new System.Drawing.Size(53, 21);
			this.btLeggi.TabIndex = 11;
			this.btLeggi.Text = "Leggi";
			this.btLeggi.UseVisualStyleBackColor = true;
			this.btLeggi.Click += new System.EventHandler(this.btLeggi_Click);
			// 
			// tbBytes
			// 
			this.tbBytes.Location = new System.Drawing.Point(148, 12);
			this.tbBytes.Mask = "0####";
			this.tbBytes.Name = "tbBytes";
			this.tbBytes.PromptChar = ' ';
			this.tbBytes.Size = new System.Drawing.Size(44, 20);
			this.tbBytes.TabIndex = 10;
			this.tbBytes.Text = "1";
			// 
			// tbIndir
			// 
			this.tbIndir.Location = new System.Drawing.Point(58, 12);
			this.tbIndir.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
			this.tbIndir.Mask = "AAAA";
			this.tbIndir.Name = "tbIndir";
			this.tbIndir.PromptChar = ' ';
			this.tbIndir.Size = new System.Drawing.Size(46, 20);
			this.tbIndir.TabIndex = 8;
			this.tbIndir.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbIndir_KeyUp);
			this.tbIndir.Leave += new System.EventHandler(this.tbIndir_Leave);
			// 
			// bt08
			// 
			this.bt08.Location = new System.Drawing.Point(11, 260);
			this.bt08.Name = "bt08";
			this.bt08.Size = new System.Drawing.Size(56, 21);
			this.bt08.TabIndex = 13;
			this.bt08.Text = "0x0008";
			this.toolTip.SetToolTip(this.bt08, "Channels active, memory counter, loop data, channels recorded");
			this.bt08.UseVisualStyleBackColor = true;
			this.bt08.Click += new System.EventHandler(this.bt08_Click);
			// 
			// bt4B
			// 
			this.bt4B.Location = new System.Drawing.Point(73, 260);
			this.bt4B.Name = "bt4B";
			this.bt4B.Size = new System.Drawing.Size(56, 21);
			this.bt4B.TabIndex = 14;
			this.bt4B.Text = "0x004B";
			this.bt4B.UseVisualStyleBackColor = true;
			this.bt4B.Click += new System.EventHandler(this.bt4B_Click);
			// 
			// bt7FF0
			// 
			this.bt7FF0.Location = new System.Drawing.Point(135, 260);
			this.bt7FF0.Name = "bt7FF0";
			this.bt7FF0.Size = new System.Drawing.Size(56, 21);
			this.bt7FF0.TabIndex = 15;
			this.bt7FF0.Text = "0x7FF8";
			this.bt7FF0.UseVisualStyleBackColor = true;
			this.bt7FF0.Click += new System.EventHandler(this.bt7FF0_Click);
			// 
			// toolTip
			// 
			this.toolTip.ShowAlways = true;
			// 
			// ShowMemory
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 285);
			this.Controls.Add(this.bt7FF0);
			this.Controls.Add(this.bt4B);
			this.Controls.Add(this.bt08);
			this.Controls.Add(this.flowLayoutPanel);
			this.Controls.Add(this.btLeggi);
			this.Controls.Add(this.tbBytes);
			this.Controls.Add(label2);
			this.Controls.Add(this.tbIndir);
			this.Controls.Add(label1);
			this.Name = "ShowMemory";
			this.Text = "ShowMemory";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
		private System.Windows.Forms.Button btLeggi;
		private System.Windows.Forms.MaskedTextBox tbBytes;
		private System.Windows.Forms.MaskedTextBox tbIndir;
		private System.Windows.Forms.Button bt08;
		private System.Windows.Forms.Button bt4B;
		private System.Windows.Forms.Button bt7FF0;
		private System.Windows.Forms.ToolTip toolTip;
	}
}