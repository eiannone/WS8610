namespace WS8610GUI
{
	partial class WriteMemory
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
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label3;
			this.btScrivi = new System.Windows.Forms.Button();
			this.tbIndir = new System.Windows.Forms.MaskedTextBox();
			this.tbBytes = new System.Windows.Forms.TextBox();
			label2 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btScrivi
			// 
			this.btScrivi.Location = new System.Drawing.Point(219, 61);
			this.btScrivi.Name = "btScrivi";
			this.btScrivi.Size = new System.Drawing.Size(53, 21);
			this.btScrivi.TabIndex = 16;
			this.btScrivi.Text = "Scrivi";
			this.btScrivi.UseVisualStyleBackColor = true;
			this.btScrivi.Click += new System.EventHandler(this.btScrivi_Click);
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(12, 38);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(36, 13);
			label2.TabIndex = 14;
			label2.Text = "Bytes:";
			// 
			// tbIndir
			// 
			this.tbIndir.Location = new System.Drawing.Point(62, 6);
			this.tbIndir.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
			this.tbIndir.Mask = "AAAA";
			this.tbIndir.Name = "tbIndir";
			this.tbIndir.PromptChar = ' ';
			this.tbIndir.Size = new System.Drawing.Size(46, 20);
			this.tbIndir.TabIndex = 13;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(12, 9);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(48, 13);
			label1.TabIndex = 12;
			label1.Text = "Indirizzo:";
			// 
			// tbBytes
			// 
			this.tbBytes.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.tbBytes.Location = new System.Drawing.Point(62, 35);
			this.tbBytes.Name = "tbBytes";
			this.tbBytes.Size = new System.Drawing.Size(210, 20);
			this.tbBytes.TabIndex = 17;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(114, 9);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(70, 13);
			label3.TabIndex = 18;
			label3.Text = "(0000 - 7FFF)";
			// 
			// WriteMemory
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 97);
			this.Controls.Add(label3);
			this.Controls.Add(this.tbBytes);
			this.Controls.Add(this.btScrivi);
			this.Controls.Add(label2);
			this.Controls.Add(this.tbIndir);
			this.Controls.Add(label1);
			this.Name = "WriteMemory";
			this.Text = "WriteMemory";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btScrivi;
		private System.Windows.Forms.MaskedTextBox tbIndir;
		private System.Windows.Forms.TextBox tbBytes;
	}
}