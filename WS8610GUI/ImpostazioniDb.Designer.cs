namespace WS8610GUI
{
	partial class ImpostazioniDb
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
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.Label label5;
			System.Windows.Forms.Label label6;
			System.Windows.Forms.Label label7;
			this.tbConfigDb = new System.Windows.Forms.TextBox();
			this.tbDbDati = new System.Windows.Forms.TextBox();
			this.tbLog = new System.Windows.Forms.TextBox();
			this.tbLogLevel = new System.Windows.Forms.TextBox();
			this.tbPortaCom = new System.Windows.Forms.TextBox();
			this.tbIntAgg = new System.Windows.Forms.TextBox();
			this.tbDtUlt = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label4 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			label7 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label1.Location = new System.Drawing.Point(13, 13);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(410, 13);
			label1.TabIndex = 0;
			label1.Text = "Percorso db contenente i dati di configurazione (impostato in WS8610Recorder.conf" +
    "):";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(13, 61);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(238, 13);
			label2.TabIndex = 2;
			label2.Text = "Cartella contenente i db con i dati e le statistiche:";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(13, 170);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(57, 13);
			label3.TabIndex = 4;
			label3.Text = "Livello log:";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(13, 111);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(62, 13);
			label4.TabIndex = 6;
			label4.Text = "Cartella log:";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(171, 170);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(58, 13);
			label5.TabIndex = 9;
			label5.Text = "Porta com:";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(328, 170);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(77, 13);
			label6.TabIndex = 11;
			label6.Text = "Intervallo agg.:";
			// 
			// tbConfigDb
			// 
			this.tbConfigDb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbConfigDb.HideSelection = false;
			this.tbConfigDb.Location = new System.Drawing.Point(16, 30);
			this.tbConfigDb.Name = "tbConfigDb";
			this.tbConfigDb.ReadOnly = true;
			this.tbConfigDb.Size = new System.Drawing.Size(527, 20);
			this.tbConfigDb.TabIndex = 1;
			// 
			// tbDbDati
			// 
			this.tbDbDati.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbDbDati.HideSelection = false;
			this.tbDbDati.Location = new System.Drawing.Point(16, 77);
			this.tbDbDati.Name = "tbDbDati";
			this.tbDbDati.ReadOnly = true;
			this.tbDbDati.Size = new System.Drawing.Size(527, 20);
			this.tbDbDati.TabIndex = 3;
			// 
			// tbLog
			// 
			this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbLog.HideSelection = false;
			this.tbLog.Location = new System.Drawing.Point(16, 127);
			this.tbLog.Name = "tbLog";
			this.tbLog.ReadOnly = true;
			this.tbLog.Size = new System.Drawing.Size(527, 20);
			this.tbLog.TabIndex = 7;
			// 
			// tbLogLevel
			// 
			this.tbLogLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbLogLevel.HideSelection = false;
			this.tbLogLevel.Location = new System.Drawing.Point(74, 167);
			this.tbLogLevel.Name = "tbLogLevel";
			this.tbLogLevel.ReadOnly = true;
			this.tbLogLevel.Size = new System.Drawing.Size(73, 20);
			this.tbLogLevel.TabIndex = 8;
			// 
			// tbPortaCom
			// 
			this.tbPortaCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbPortaCom.HideSelection = false;
			this.tbPortaCom.Location = new System.Drawing.Point(232, 167);
			this.tbPortaCom.Name = "tbPortaCom";
			this.tbPortaCom.ReadOnly = true;
			this.tbPortaCom.Size = new System.Drawing.Size(73, 20);
			this.tbPortaCom.TabIndex = 10;
			// 
			// tbIntAgg
			// 
			this.tbIntAgg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbIntAgg.HideSelection = false;
			this.tbIntAgg.Location = new System.Drawing.Point(409, 167);
			this.tbIntAgg.Name = "tbIntAgg";
			this.tbIntAgg.ReadOnly = true;
			this.tbIntAgg.Size = new System.Drawing.Size(73, 20);
			this.tbIntAgg.TabIndex = 12;
			// 
			// tbDtUlt
			// 
			this.tbDtUlt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbDtUlt.HideSelection = false;
			this.tbDtUlt.Location = new System.Drawing.Point(116, 207);
			this.tbDtUlt.Name = "tbDtUlt";
			this.tbDtUlt.ReadOnly = true;
			this.tbDtUlt.Size = new System.Drawing.Size(101, 20);
			this.tbDtUlt.TabIndex = 14;
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(13, 210);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(101, 13);
			label7.TabIndex = 13;
			label7.Text = "Ultima registrazione:";
			// 
			// ImpostazioniDb
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(555, 245);
			this.Controls.Add(this.tbDtUlt);
			this.Controls.Add(label7);
			this.Controls.Add(this.tbIntAgg);
			this.Controls.Add(label6);
			this.Controls.Add(this.tbPortaCom);
			this.Controls.Add(label5);
			this.Controls.Add(this.tbLogLevel);
			this.Controls.Add(this.tbLog);
			this.Controls.Add(label4);
			this.Controls.Add(label3);
			this.Controls.Add(this.tbDbDati);
			this.Controls.Add(label2);
			this.Controls.Add(this.tbConfigDb);
			this.Controls.Add(label1);
			this.Name = "ImpostazioniDb";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "ImpostazioniDb";
			this.Shown += new System.EventHandler(this.ImpostazioniDb_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbConfigDb;
		private System.Windows.Forms.TextBox tbDbDati;
		private System.Windows.Forms.TextBox tbLog;
		private System.Windows.Forms.TextBox tbLogLevel;
		private System.Windows.Forms.TextBox tbPortaCom;
		private System.Windows.Forms.TextBox tbIntAgg;
		private System.Windows.Forms.TextBox tbDtUlt;
	}
}