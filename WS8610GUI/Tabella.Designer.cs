namespace WS8610GUI
{
	partial class Tabella
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgDati = new System.Windows.Forms.DataGridView();
            this.Pos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DataOra = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Temp0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Umid0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Temp1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Umid1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Temp2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Umid2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Temp3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Umid3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgDati)).BeginInit();
            this.SuspendLayout();
            // 
            // dgDati
            // 
            this.dgDati.AllowUserToAddRows = false;
            this.dgDati.AllowUserToDeleteRows = false;
            this.dgDati.AllowUserToResizeRows = false;
            this.dgDati.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDati.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgDati.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDati.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Pos,
            this.DataOra,
            this.Temp0,
            this.Umid0,
            this.Temp1,
            this.Umid1,
            this.Temp2,
            this.Umid2,
            this.Temp3,
            this.Umid3});
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgDati.DefaultCellStyle = dataGridViewCellStyle12;
            this.dgDati.Location = new System.Drawing.Point(1, 29);
            this.dgDati.Name = "dgDati";
            this.dgDati.ReadOnly = true;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDati.RowHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgDati.RowHeadersVisible = false;
            this.dgDati.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgDati.Size = new System.Drawing.Size(483, 473);
            this.dgDati.TabIndex = 0;
            this.dgDati.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDati_CellClick);
            // 
            // Pos
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.Pos.DefaultCellStyle = dataGridViewCellStyle2;
            this.Pos.HeaderText = "Pos.";
            this.Pos.Name = "Pos";
            this.Pos.ReadOnly = true;
            this.Pos.Width = 40;
            // 
            // DataOra
            // 
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.DataOra.DefaultCellStyle = dataGridViewCellStyle3;
            this.DataOra.HeaderText = "Data/ora";
            this.DataOra.Name = "DataOra";
            this.DataOra.ReadOnly = true;
            this.DataOra.Width = 110;
            // 
            // Temp0
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.MistyRose;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.Temp0.DefaultCellStyle = dataGridViewCellStyle4;
            this.Temp0.HeaderText = "T.0";
            this.Temp0.Name = "Temp0";
            this.Temp0.ReadOnly = true;
            this.Temp0.Width = 40;
            // 
            // Umid0
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.MistyRose;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.Umid0.DefaultCellStyle = dataGridViewCellStyle5;
            this.Umid0.HeaderText = "U.0";
            this.Umid0.Name = "Umid0";
            this.Umid0.ReadOnly = true;
            this.Umid0.Width = 40;
            // 
            // Temp1
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.Temp1.DefaultCellStyle = dataGridViewCellStyle6;
            this.Temp1.HeaderText = "T.1";
            this.Temp1.Name = "Temp1";
            this.Temp1.ReadOnly = true;
            this.Temp1.Width = 40;
            // 
            // Umid1
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.Umid1.DefaultCellStyle = dataGridViewCellStyle7;
            this.Umid1.HeaderText = "U.1";
            this.Umid1.Name = "Umid1";
            this.Umid1.ReadOnly = true;
            this.Umid1.Width = 40;
            // 
            // Temp2
            // 
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(255)))), ((int)(((byte)(220)))));
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.Temp2.DefaultCellStyle = dataGridViewCellStyle8;
            this.Temp2.HeaderText = "T.2";
            this.Temp2.Name = "Temp2";
            this.Temp2.ReadOnly = true;
            this.Temp2.Width = 40;
            // 
            // Umid2
            // 
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(255)))), ((int)(((byte)(220)))));
            dataGridViewCellStyle9.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.Umid2.DefaultCellStyle = dataGridViewCellStyle9;
            this.Umid2.HeaderText = "U.2";
            this.Umid2.Name = "Umid2";
            this.Umid2.ReadOnly = true;
            this.Umid2.Width = 40;
            // 
            // Temp3
            // 
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle10.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.Temp3.DefaultCellStyle = dataGridViewCellStyle10;
            this.Temp3.HeaderText = "T.3";
            this.Temp3.Name = "Temp3";
            this.Temp3.ReadOnly = true;
            this.Temp3.Width = 40;
            // 
            // Umid3
            // 
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle11.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.Umid3.DefaultCellStyle = dataGridViewCellStyle11;
            this.Umid3.HeaderText = "U.3";
            this.Umid3.Name = "Umid3";
            this.Umid3.ReadOnly = true;
            this.Umid3.Width = 40;
            // 
            // Tabella
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 502);
            this.Controls.Add(this.dgDati);
            this.Name = "Tabella";
            this.Text = "Tabella";
            ((System.ComponentModel.ISupportInitialize)(this.dgDati)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgDati;
		private System.Windows.Forms.DataGridViewTextBoxColumn Pos;
		private System.Windows.Forms.DataGridViewTextBoxColumn DataOra;
		private System.Windows.Forms.DataGridViewTextBoxColumn Temp0;
		private System.Windows.Forms.DataGridViewTextBoxColumn Umid0;
		private System.Windows.Forms.DataGridViewTextBoxColumn Temp1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Umid1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Temp2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Umid2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Temp3;
		private System.Windows.Forms.DataGridViewTextBoxColumn Umid3;
	}
}