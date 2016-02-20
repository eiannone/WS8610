namespace WS8610GUI
{
	partial class EliminaMisure
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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			this.dtDa = new System.Windows.Forms.DateTimePicker();
			this.btOK = new System.Windows.Forms.Button();
			this.btAnnulla = new System.Windows.Forms.Button();
			this.dtA = new System.Windows.Forms.DateTimePicker();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(9, 9);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(23, 13);
			label1.TabIndex = 29;
			label1.Text = "Dal";
			// 
			// dtDa
			// 
			this.dtDa.CustomFormat = "dd/MM/yyyy HH:mm";
			this.dtDa.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtDa.Location = new System.Drawing.Point(33, 6);
			this.dtDa.Name = "dtDa";
			this.dtDa.Size = new System.Drawing.Size(125, 20);
			this.dtDa.TabIndex = 30;
			// 
			// btOK
			// 
			this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btOK.Location = new System.Drawing.Point(179, 42);
			this.btOK.Name = "btOK";
			this.btOK.Size = new System.Drawing.Size(59, 23);
			this.btOK.TabIndex = 31;
			this.btOK.Text = "OK";
			this.btOK.UseVisualStyleBackColor = true;
			this.btOK.Click += new System.EventHandler(this.btOK_Click);
			// 
			// btAnnulla
			// 
			this.btAnnulla.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btAnnulla.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btAnnulla.Location = new System.Drawing.Point(245, 42);
			this.btAnnulla.Name = "btAnnulla";
			this.btAnnulla.Size = new System.Drawing.Size(57, 23);
			this.btAnnulla.TabIndex = 32;
			this.btAnnulla.Text = "Annulla";
			this.btAnnulla.UseVisualStyleBackColor = true;
			this.btAnnulla.Click += new System.EventHandler(this.btAnnulla_Click);
			// 
			// dtA
			// 
			this.dtA.CustomFormat = "dd/MM/yyyy HH:mm";
			this.dtA.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtA.Location = new System.Drawing.Point(181, 6);
			this.dtA.Name = "dtA";
			this.dtA.Size = new System.Drawing.Size(125, 20);
			this.dtA.TabIndex = 34;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(166, 9);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(15, 13);
			label2.TabIndex = 33;
			label2.Text = "al";
			// 
			// EliminaMisure
			// 
			this.AcceptButton = this.btOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btAnnulla;
			this.ClientSize = new System.Drawing.Size(321, 77);
			this.Controls.Add(this.dtA);
			this.Controls.Add(label2);
			this.Controls.Add(this.btAnnulla);
			this.Controls.Add(this.btOK);
			this.Controls.Add(this.dtDa);
			this.Controls.Add(label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.Name = "EliminaMisure";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Elimina misurazioni";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DateTimePicker dtDa;
		private System.Windows.Forms.Button btOK;
		private System.Windows.Forms.Button btAnnulla;
		private System.Windows.Forms.DateTimePicker dtA;
	}
}