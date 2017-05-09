namespace WS8610GUI
{
	partial class Database
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
            System.Windows.Forms.Label lbUltimo;
            System.Windows.Forms.Label lbOre;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Database));
            this.lbOra = new System.Windows.Forms.Label();
            this.lbData = new System.Windows.Forms.Label();
            this.btAggiorna = new System.Windows.Forms.Button();
            this.cbOffset = new System.Windows.Forms.CheckBox();
            this.tbOffset = new System.Windows.Forms.MaskedTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbGiorni = new System.Windows.Forms.TextBox();
            this.tbA = new System.Windows.Forms.TextBox();
            this.tbDa = new System.Windows.Forms.TextBox();
            this.rbDaA = new System.Windows.Forms.RadioButton();
            this.rbTutti = new System.Windows.Forms.RadioButton();
            this.cbOffsetG = new System.Windows.Forms.CheckBox();
            this.lbMemOra = new System.Windows.Forms.Label();
            this.lbMem2 = new System.Windows.Forms.Label();
            this.lbMemDt = new System.Windows.Forms.Label();
            this.lbMem = new System.Windows.Forms.Label();
            lbUltimo = new System.Windows.Forms.Label();
            lbOre = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbUltimo
            // 
            lbUltimo.AutoSize = true;
            lbUltimo.Location = new System.Drawing.Point(9, 13);
            lbUltimo.Name = "lbUltimo";
            lbUltimo.Size = new System.Drawing.Size(72, 13);
            lbUltimo.TabIndex = 0;
            lbUltimo.Text = "Ultimo record:";
            // 
            // lbOre
            // 
            lbOre.AutoSize = true;
            lbOre.Location = new System.Drawing.Point(180, 13);
            lbOre.Name = "lbOre";
            lbOre.Size = new System.Drawing.Size(25, 13);
            lbOre.TabIndex = 22;
            lbOre.Text = "ore:";
            // 
            // lbOra
            // 
            this.lbOra.AutoSize = true;
            this.lbOra.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOra.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbOra.Location = new System.Drawing.Point(200, 11);
            this.lbOra.Name = "lbOra";
            this.lbOra.Size = new System.Drawing.Size(43, 15);
            this.lbOra.TabIndex = 23;
            this.lbOra.Text = "12:30";
            // 
            // lbData
            // 
            this.lbData.AutoSize = true;
            this.lbData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbData.ForeColor = System.Drawing.SystemColors.Desktop;
            this.lbData.Location = new System.Drawing.Point(99, 11);
            this.lbData.Name = "lbData";
            this.lbData.Size = new System.Drawing.Size(79, 15);
            this.lbData.TabIndex = 21;
            this.lbData.Text = "01/01/2004";
            // 
            // btAggiorna
            // 
            this.btAggiorna.Image = ((System.Drawing.Image)(resources.GetObject("btAggiorna.Image")));
            this.btAggiorna.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btAggiorna.Location = new System.Drawing.Point(153, 192);
            this.btAggiorna.Name = "btAggiorna";
            this.btAggiorna.Size = new System.Drawing.Size(90, 23);
            this.btAggiorna.TabIndex = 26;
            this.btAggiorna.Text = "Scarica dati";
            this.btAggiorna.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btAggiorna.UseVisualStyleBackColor = true;
            this.btAggiorna.Click += new System.EventHandler(this.btAggiorna_Click);
            // 
            // cbOffset
            // 
            this.cbOffset.AutoSize = true;
            this.cbOffset.Enabled = false;
            this.cbOffset.Location = new System.Drawing.Point(10, 77);
            this.cbOffset.Name = "cbOffset";
            this.cbOffset.Size = new System.Drawing.Size(128, 17);
            this.cbOffset.TabIndex = 29;
            this.cbOffset.Text = "Correzione (± hh:mm):";
            this.cbOffset.UseVisualStyleBackColor = true;
            // 
            // tbOffset
            // 
            this.tbOffset.Enabled = false;
            this.tbOffset.Location = new System.Drawing.Point(135, 75);
            this.tbOffset.Mask = "#00:00";
            this.tbOffset.Name = "tbOffset";
            this.tbOffset.Size = new System.Drawing.Size(41, 20);
            this.tbOffset.TabIndex = 30;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbGiorni);
            this.groupBox1.Controls.Add(this.tbA);
            this.groupBox1.Controls.Add(this.tbOffset);
            this.groupBox1.Controls.Add(this.tbDa);
            this.groupBox1.Controls.Add(this.cbOffset);
            this.groupBox1.Controls.Add(this.rbDaA);
            this.groupBox1.Controls.Add(this.rbTutti);
            this.groupBox1.Location = new System.Drawing.Point(12, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(231, 128);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scarica dati";
            // 
            // tbGiorni
            // 
            this.tbGiorni.Enabled = false;
            this.tbGiorni.Location = new System.Drawing.Point(135, 100);
            this.tbGiorni.Name = "tbGiorni";
            this.tbGiorni.Size = new System.Drawing.Size(41, 20);
            this.tbGiorni.TabIndex = 33;
            // 
            // tbA
            // 
            this.tbA.Location = new System.Drawing.Point(181, 43);
            this.tbA.Name = "tbA";
            this.tbA.Size = new System.Drawing.Size(36, 20);
            this.tbA.TabIndex = 32;
            this.tbA.Text = "100";
            this.tbA.Click += new System.EventHandler(this.tbDa_Enter);
            this.tbA.Enter += new System.EventHandler(this.tbDa_Enter);
            // 
            // tbDa
            // 
            this.tbDa.Location = new System.Drawing.Point(127, 43);
            this.tbDa.Name = "tbDa";
            this.tbDa.Size = new System.Drawing.Size(36, 20);
            this.tbDa.TabIndex = 2;
            this.tbDa.Text = "0";
            this.tbDa.Click += new System.EventHandler(this.tbDa_Enter);
            this.tbDa.Enter += new System.EventHandler(this.tbDa_Enter);
            // 
            // rbDaA
            // 
            this.rbDaA.AutoSize = true;
            this.rbDaA.Location = new System.Drawing.Point(11, 44);
            this.rbDaA.Name = "rbDaA";
            this.rbDaA.Size = new System.Drawing.Size(169, 17);
            this.rbDaA.TabIndex = 1;
            this.rbDaA.Text = "intervallo record da                a";
            this.rbDaA.UseVisualStyleBackColor = true;
            // 
            // rbTutti
            // 
            this.rbTutti.AutoSize = true;
            this.rbTutti.Checked = true;
            this.rbTutti.Location = new System.Drawing.Point(11, 20);
            this.rbTutti.Name = "rbTutti";
            this.rbTutti.Size = new System.Drawing.Size(109, 17);
            this.rbTutti.TabIndex = 0;
            this.rbTutti.TabStop = true;
            this.rbTutti.Text = "tutti i nuovi record";
            this.rbTutti.UseVisualStyleBackColor = true;
            this.rbTutti.CheckedChanged += new System.EventHandler(this.rbTutti_CheckedChanged);
            // 
            // cbOffsetG
            // 
            this.cbOffsetG.AutoSize = true;
            this.cbOffsetG.Enabled = false;
            this.cbOffsetG.Location = new System.Drawing.Point(22, 160);
            this.cbOffsetG.Name = "cbOffsetG";
            this.cbOffsetG.Size = new System.Drawing.Size(107, 17);
            this.cbOffsetG.TabIndex = 32;
            this.cbOffsetG.Text = "Correzione giorni:";
            this.cbOffsetG.UseVisualStyleBackColor = true;
            // 
            // lbMemOra
            // 
            this.lbMemOra.AutoSize = true;
            this.lbMemOra.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMemOra.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lbMemOra.Location = new System.Drawing.Point(200, 33);
            this.lbMemOra.Name = "lbMemOra";
            this.lbMemOra.Size = new System.Drawing.Size(43, 15);
            this.lbMemOra.TabIndex = 36;
            this.lbMemOra.Text = "12:30";
            // 
            // lbMem2
            // 
            this.lbMem2.AutoSize = true;
            this.lbMem2.Location = new System.Drawing.Point(180, 35);
            this.lbMem2.Name = "lbMem2";
            this.lbMem2.Size = new System.Drawing.Size(25, 13);
            this.lbMem2.TabIndex = 35;
            this.lbMem2.Text = "ore:";
            // 
            // lbMemDt
            // 
            this.lbMemDt.AutoSize = true;
            this.lbMemDt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMemDt.ForeColor = System.Drawing.SystemColors.Desktop;
            this.lbMemDt.Location = new System.Drawing.Point(99, 33);
            this.lbMemDt.Name = "lbMemDt";
            this.lbMemDt.Size = new System.Drawing.Size(79, 15);
            this.lbMemDt.TabIndex = 34;
            this.lbMemDt.Text = "01/01/2004";
            // 
            // lbMem
            // 
            this.lbMem.AutoSize = true;
            this.lbMem.Location = new System.Drawing.Point(9, 35);
            this.lbMem.Name = "lbMem";
            this.lbMem.Size = new System.Drawing.Size(92, 13);
            this.lbMem.TabIndex = 33;
            this.lbMem.Text = "Ultimo in memoria:";
            // 
            // Database
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 229);
            this.Controls.Add(this.lbMemOra);
            this.Controls.Add(this.lbMem2);
            this.Controls.Add(this.lbMemDt);
            this.Controls.Add(this.lbMem);
            this.Controls.Add(this.cbOffsetG);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btAggiorna);
            this.Controls.Add(this.lbOra);
            this.Controls.Add(lbOre);
            this.Controls.Add(this.lbData);
            this.Controls.Add(lbUltimo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Database";
            this.ShowIcon = false;
            this.Text = "Database";
            this.Load += new System.EventHandler(this.Database_Load);
            this.Shown += new System.EventHandler(this.Database_Load);
            this.Enter += new System.EventHandler(this.Database_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lbOra;
		private System.Windows.Forms.Label lbData;
		private System.Windows.Forms.Button btAggiorna;
		private System.Windows.Forms.CheckBox cbOffset;
		private System.Windows.Forms.MaskedTextBox tbOffset;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox tbDa;
		private System.Windows.Forms.RadioButton rbDaA;
		private System.Windows.Forms.RadioButton rbTutti;
		private System.Windows.Forms.TextBox tbA;
		private System.Windows.Forms.TextBox tbGiorni;
		private System.Windows.Forms.CheckBox cbOffsetG;
		private System.Windows.Forms.Label lbMemOra;
		private System.Windows.Forms.Label lbMemDt;
		private System.Windows.Forms.Label lbMem;
		private System.Windows.Forms.Label lbMem2;
	}
}