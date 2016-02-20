namespace WS8610GUI
{
	partial class HexByte
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.lbAddr = new System.Windows.Forms.Label();
			this.lbByte = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lbAddr
			// 
			this.lbAddr.AutoSize = true;
			this.lbAddr.BackColor = System.Drawing.SystemColors.Control;
			this.lbAddr.Font = new System.Drawing.Font("Courier New", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbAddr.Location = new System.Drawing.Point(1, 20);
			this.lbAddr.Name = "lbAddr";
			this.lbAddr.Size = new System.Drawing.Size(25, 12);
			this.lbAddr.TabIndex = 9;
			this.lbAddr.Text = "0064";
			// 
			// lbByte
			// 
			this.lbByte.AutoSize = true;
			this.lbByte.BackColor = System.Drawing.SystemColors.Control;
			this.lbByte.Font = new System.Drawing.Font("Courier New", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbByte.Location = new System.Drawing.Point(1, 1);
			this.lbByte.Name = "lbByte";
			this.lbByte.Size = new System.Drawing.Size(26, 17);
			this.lbByte.TabIndex = 8;
			this.lbByte.Text = "99";
			// 
			// HexByte
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLight;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.lbAddr);
			this.Controls.Add(this.lbByte);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.Name = "HexByte";
			this.Size = new System.Drawing.Size(28, 33);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lbAddr;
		private System.Windows.Forms.Label lbByte;
	}
}
