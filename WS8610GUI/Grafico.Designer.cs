namespace WS8610GUI
{
	partial class Grafico
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
			System.Windows.Forms.GroupBox gbSensori;
			System.Windows.Forms.GroupBox groupBox1;
			System.Windows.Forms.Label lbDal;
			System.Windows.Forms.Label lbAl;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Grafico));
			this.cb4 = new System.Windows.Forms.CheckBox();
			this.cbBase = new System.Windows.Forms.CheckBox();
			this.cbCamera = new System.Windows.Forms.CheckBox();
			this.cbEsterno = new System.Windows.Forms.CheckBox();
			this.cbUmid = new System.Windows.Forms.CheckBox();
			this.cbTemp = new System.Windows.Forms.CheckBox();
			this.zg = new ZedGraph.ZedGraphControl();
			this.btUlt12 = new System.Windows.Forms.Button();
			this.cbUlt24 = new System.Windows.Forms.Button();
			this.bt2gg = new System.Windows.Forms.Button();
			this.btSett = new System.Windows.Forms.Button();
			this.dtDa = new System.Windows.Forms.DateTimePicker();
			this.dtA = new System.Windows.Forms.DateTimePicker();
			this.btAggiorna = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			gbSensori = new System.Windows.Forms.GroupBox();
			groupBox1 = new System.Windows.Forms.GroupBox();
			lbDal = new System.Windows.Forms.Label();
			lbAl = new System.Windows.Forms.Label();
			gbSensori.SuspendLayout();
			groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbSensori
			// 
			gbSensori.Controls.Add(this.cb4);
			gbSensori.Controls.Add(this.cbBase);
			gbSensori.Controls.Add(this.cbCamera);
			gbSensori.Controls.Add(this.cbEsterno);
			gbSensori.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			gbSensori.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			gbSensori.Location = new System.Drawing.Point(8, 2);
			gbSensori.Name = "gbSensori";
			gbSensori.Size = new System.Drawing.Size(337, 36);
			gbSensori.TabIndex = 1;
			gbSensori.TabStop = false;
			gbSensori.Text = "sensori";
			// 
			// cb4
			// 
			this.cb4.AutoSize = true;
			this.cb4.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cb4.ForeColor = System.Drawing.Color.DimGray;
			this.cb4.Location = new System.Drawing.Point(247, 15);
			this.cb4.Name = "cb4";
			this.cb4.Size = new System.Drawing.Size(76, 18);
			this.cb4.TabIndex = 3;
			this.cb4.Tag = "S3";
			this.cb4.Text = "Sensore 4";
			this.cb4.UseVisualStyleBackColor = true;
			this.cb4.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
			// 
			// cbBase
			// 
			this.cbBase.AutoSize = true;
			this.cbBase.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbBase.ForeColor = System.Drawing.Color.DarkRed;
			this.cbBase.Location = new System.Drawing.Point(157, 15);
			this.cbBase.Name = "cbBase";
			this.cbBase.Size = new System.Drawing.Size(75, 18);
			this.cbBase.TabIndex = 2;
			this.cbBase.Tag = "S0";
			this.cbBase.Text = "Cameretta";
			this.cbBase.UseVisualStyleBackColor = true;
			this.cbBase.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
			// 
			// cbCamera
			// 
			this.cbCamera.AutoSize = true;
			this.cbCamera.Checked = true;
			this.cbCamera.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbCamera.Font = new System.Drawing.Font("Arial", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbCamera.ForeColor = System.Drawing.Color.DarkGreen;
			this.cbCamera.Location = new System.Drawing.Point(82, 15);
			this.cbCamera.Name = "cbCamera";
			this.cbCamera.Size = new System.Drawing.Size(69, 18);
			this.cbCamera.TabIndex = 1;
			this.cbCamera.Tag = "S2";
			this.cbCamera.Text = "C. Letto";
			this.cbCamera.UseVisualStyleBackColor = true;
			this.cbCamera.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
			// 
			// cbEsterno
			// 
			this.cbEsterno.AutoSize = true;
			this.cbEsterno.Checked = true;
			this.cbEsterno.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEsterno.Font = new System.Drawing.Font("Arial", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbEsterno.ForeColor = System.Drawing.Color.DarkBlue;
			this.cbEsterno.Location = new System.Drawing.Point(9, 15);
			this.cbEsterno.Name = "cbEsterno";
			this.cbEsterno.Size = new System.Drawing.Size(69, 18);
			this.cbEsterno.TabIndex = 0;
			this.cbEsterno.Tag = "S1";
			this.cbEsterno.Text = "Esterno";
			this.cbEsterno.UseVisualStyleBackColor = true;
			this.cbEsterno.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(this.cbUmid);
			groupBox1.Controls.Add(this.cbTemp);
			groupBox1.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			groupBox1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			groupBox1.Location = new System.Drawing.Point(360, 2);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(162, 36);
			groupBox1.TabIndex = 2;
			groupBox1.TabStop = false;
			groupBox1.Text = "dati";
			// 
			// cbUmid
			// 
			this.cbUmid.AutoSize = true;
			this.cbUmid.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbUmid.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cbUmid.Location = new System.Drawing.Point(96, 15);
			this.cbUmid.Name = "cbUmid";
			this.cbUmid.Size = new System.Drawing.Size(60, 18);
			this.cbUmid.TabIndex = 1;
			this.cbUmid.Tag = "S2";
			this.cbUmid.Text = "Umidità";
			this.cbUmid.UseVisualStyleBackColor = true;
			this.cbUmid.CheckedChanged += new System.EventHandler(this.cbUmid_CheckedChanged);
			// 
			// cbTemp
			// 
			this.cbTemp.AutoSize = true;
			this.cbTemp.Checked = true;
			this.cbTemp.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbTemp.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbTemp.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cbTemp.Location = new System.Drawing.Point(9, 15);
			this.cbTemp.Name = "cbTemp";
			this.cbTemp.Size = new System.Drawing.Size(86, 18);
			this.cbTemp.TabIndex = 0;
			this.cbTemp.Tag = "S1";
			this.cbTemp.Text = "Temperatura";
			this.cbTemp.UseVisualStyleBackColor = true;
			this.cbTemp.CheckedChanged += new System.EventHandler(this.cbTemp_CheckedChanged);
			// 
			// lbDal
			// 
			lbDal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			lbDal.AutoSize = true;
			lbDal.Location = new System.Drawing.Point(285, 388);
			lbDal.Name = "lbDal";
			lbDal.Size = new System.Drawing.Size(25, 14);
			lbDal.TabIndex = 9;
			lbDal.Text = "Dal:";
			// 
			// lbAl
			// 
			lbAl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			lbAl.AutoSize = true;
			lbAl.Location = new System.Drawing.Point(433, 388);
			lbAl.Name = "lbAl";
			lbAl.Size = new System.Drawing.Size(18, 14);
			lbAl.TabIndex = 10;
			lbAl.Text = "al:";
			// 
			// zg
			// 
			this.zg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.zg.AutoSize = true;
			this.zg.IsShowPointValues = true;
			this.zg.Location = new System.Drawing.Point(0, 47);
			this.zg.Name = "zg";
			this.zg.ScrollGrace = 0D;
			this.zg.ScrollMaxX = 0D;
			this.zg.ScrollMaxY = 0D;
			this.zg.ScrollMaxY2 = 0D;
			this.zg.ScrollMinX = 0D;
			this.zg.ScrollMinY = 0D;
			this.zg.ScrollMinY2 = 0D;
			this.zg.Size = new System.Drawing.Size(623, 324);
			this.zg.TabIndex = 0;
			this.zg.Click += new System.EventHandler(this.zg_Click);
			this.zg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.zg_MouseMove);
			// 
			// btUlt12
			// 
			this.btUlt12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btUlt12.Image = ((System.Drawing.Image)(resources.GetObject("btUlt12.Image")));
			this.btUlt12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btUlt12.Location = new System.Drawing.Point(8, 385);
			this.btUlt12.Name = "btUlt12";
			this.btUlt12.Size = new System.Drawing.Size(62, 21);
			this.btUlt12.TabIndex = 3;
			this.btUlt12.Text = "12 ore";
			this.btUlt12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btUlt12.UseVisualStyleBackColor = true;
			this.btUlt12.Click += new System.EventHandler(this.btUlt12_Click);
			// 
			// cbUlt24
			// 
			this.cbUlt24.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbUlt24.Image = ((System.Drawing.Image)(resources.GetObject("cbUlt24.Image")));
			this.cbUlt24.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cbUlt24.Location = new System.Drawing.Point(76, 385);
			this.cbUlt24.Name = "cbUlt24";
			this.cbUlt24.Size = new System.Drawing.Size(62, 21);
			this.cbUlt24.TabIndex = 4;
			this.cbUlt24.Text = "24 ore";
			this.cbUlt24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cbUlt24.UseVisualStyleBackColor = true;
			this.cbUlt24.Click += new System.EventHandler(this.cbUlt24_Click);
			// 
			// bt2gg
			// 
			this.bt2gg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.bt2gg.Image = ((System.Drawing.Image)(resources.GetObject("bt2gg.Image")));
			this.bt2gg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.bt2gg.Location = new System.Drawing.Point(144, 385);
			this.bt2gg.Name = "bt2gg";
			this.bt2gg.Size = new System.Drawing.Size(62, 21);
			this.bt2gg.TabIndex = 5;
			this.bt2gg.Text = "2 gg ";
			this.bt2gg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.bt2gg.UseVisualStyleBackColor = true;
			this.bt2gg.Click += new System.EventHandler(this.bt2gg_Click);
			// 
			// btSett
			// 
			this.btSett.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btSett.Image = ((System.Drawing.Image)(resources.GetObject("btSett.Image")));
			this.btSett.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btSett.Location = new System.Drawing.Point(212, 385);
			this.btSett.Name = "btSett";
			this.btSett.Size = new System.Drawing.Size(62, 21);
			this.btSett.TabIndex = 6;
			this.btSett.Text = "7 gg ";
			this.btSett.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btSett.UseVisualStyleBackColor = true;
			this.btSett.Click += new System.EventHandler(this.btSett_Click);
			// 
			// dtDa
			// 
			this.dtDa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.dtDa.CustomFormat = "dd/MM/yyyy HH:mm";
			this.dtDa.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtDa.Location = new System.Drawing.Point(311, 385);
			this.dtDa.Name = "dtDa";
			this.dtDa.Size = new System.Drawing.Size(122, 20);
			this.dtDa.TabIndex = 7;
			// 
			// dtA
			// 
			this.dtA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.dtA.CustomFormat = "dd/MM/yyyy HH:mm";
			this.dtA.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtA.Location = new System.Drawing.Point(453, 385);
			this.dtA.Name = "dtA";
			this.dtA.Size = new System.Drawing.Size(122, 20);
			this.dtA.TabIndex = 8;
			// 
			// btAggiorna
			// 
			this.btAggiorna.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btAggiorna.Image = ((System.Drawing.Image)(resources.GetObject("btAggiorna.Image")));
			this.btAggiorna.Location = new System.Drawing.Point(581, 384);
			this.btAggiorna.Name = "btAggiorna";
			this.btAggiorna.Size = new System.Drawing.Size(31, 21);
			this.btAggiorna.TabIndex = 11;
			this.btAggiorna.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btAggiorna.UseVisualStyleBackColor = true;
			this.btAggiorna.Click += new System.EventHandler(this.btAggiorna_Click);
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay = 2000;
			this.toolTip1.InitialDelay = 500;
			this.toolTip1.ReshowDelay = 100;
			// 
			// Grafico
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 420);
			this.Controls.Add(this.btAggiorna);
			this.Controls.Add(lbAl);
			this.Controls.Add(lbDal);
			this.Controls.Add(this.dtA);
			this.Controls.Add(this.dtDa);
			this.Controls.Add(this.btSett);
			this.Controls.Add(this.bt2gg);
			this.Controls.Add(this.cbUlt24);
			this.Controls.Add(this.btUlt12);
			this.Controls.Add(groupBox1);
			this.Controls.Add(gbSensori);
			this.Controls.Add(this.zg);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "Grafico";
			this.Text = "Grafico";
			gbSensori.ResumeLayout(false);
			gbSensori.PerformLayout();
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private ZedGraph.ZedGraphControl zg;
		private System.Windows.Forms.CheckBox cbEsterno;
		private System.Windows.Forms.CheckBox cb4;
		private System.Windows.Forms.CheckBox cbBase;
		private System.Windows.Forms.CheckBox cbCamera;
		private System.Windows.Forms.CheckBox cbUmid;
		private System.Windows.Forms.CheckBox cbTemp;
		private System.Windows.Forms.Button btUlt12;
		private System.Windows.Forms.Button cbUlt24;
		private System.Windows.Forms.Button bt2gg;
		private System.Windows.Forms.Button btSett;
		private System.Windows.Forms.DateTimePicker dtDa;
		private System.Windows.Forms.DateTimePicker dtA;
		private System.Windows.Forms.Button btAggiorna;
		private System.Windows.Forms.ToolTip toolTip1;

	}
}