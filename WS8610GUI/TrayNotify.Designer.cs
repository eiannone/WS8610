namespace WS8610GUI
{
	partial class TrayNotify
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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrayNotify));
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btChiudi = new System.Windows.Forms.Button();
			this.lkLog = new System.Windows.Forms.LinkLabel();
			this.dgLog = new System.Windows.Forms.DataGridView();
			this.ora = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.msg = new System.Windows.Forms.DataGridViewTextBoxColumn();
			label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgLog)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label1.ForeColor = System.Drawing.Color.White;
			label1.Location = new System.Drawing.Point(23, 4);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(97, 13);
			label1.TabIndex = 4;
			label1.Text = "Notifiche WS8610:";
			// 
			// timer
			// 
			this.timer.Interval = 50;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::WS8610GUI.Properties.Resources.WS8610ico;
			this.pictureBox1.Location = new System.Drawing.Point(4, 1);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(18, 18);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			// 
			// btChiudi
			// 
			this.btChiudi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.btChiudi.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btChiudi.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.btChiudi.FlatAppearance.BorderSize = 0;
			this.btChiudi.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(41)))), ((int)(((byte)(31)))));
			this.btChiudi.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(41)))), ((int)(((byte)(31)))));
			this.btChiudi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btChiudi.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btChiudi.ForeColor = System.Drawing.Color.White;
			this.btChiudi.Location = new System.Drawing.Point(277, 0);
			this.btChiudi.Name = "btChiudi";
			this.btChiudi.Size = new System.Drawing.Size(18, 20);
			this.btChiudi.TabIndex = 6;
			this.btChiudi.Text = "X";
			this.btChiudi.UseVisualStyleBackColor = false;
			this.btChiudi.Click += new System.EventHandler(this.btChiudi_Click);
			// 
			// lkLog
			// 
			this.lkLog.ActiveLinkColor = System.Drawing.Color.Firebrick;
			this.lkLog.AutoSize = true;
			this.lkLog.BackColor = System.Drawing.Color.WhiteSmoke;
			this.lkLog.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.lkLog.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lkLog.Location = new System.Drawing.Point(230, 59);
			this.lkLog.Name = "lkLog";
			this.lkLog.Size = new System.Drawing.Size(55, 13);
			this.lkLog.TabIndex = 8;
			this.lkLog.TabStop = true;
			this.lkLog.Text = "mostra log";
			this.lkLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lkLog.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.lkLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lkLog_LinkClicked);
			// 
			// dgLog
			// 
			this.dgLog.AllowUserToAddRows = false;
			this.dgLog.AllowUserToDeleteRows = false;
			this.dgLog.AllowUserToResizeColumns = false;
			this.dgLog.AllowUserToResizeRows = false;
			this.dgLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgLog.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.dgLog.BackgroundColor = System.Drawing.Color.WhiteSmoke;
			this.dgLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgLog.ColumnHeadersVisible = false;
			this.dgLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ora,
            this.msg});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgLog.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgLog.Location = new System.Drawing.Point(4, 21);
			this.dgLog.Name = "dgLog";
			this.dgLog.ReadOnly = true;
			this.dgLog.RowHeadersVisible = false;
			this.dgLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgLog.Size = new System.Drawing.Size(292, 54);
			this.dgLog.TabIndex = 9;
			// 
			// ora
			// 
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
			this.ora.DefaultCellStyle = dataGridViewCellStyle1;
			this.ora.HeaderText = "ora";
			this.ora.Name = "ora";
			this.ora.ReadOnly = true;
			this.ora.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ora.Width = 38;
			// 
			// msg
			// 
			this.msg.HeaderText = "msg";
			this.msg.Name = "msg";
			this.msg.ReadOnly = true;
			this.msg.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.msg.Width = 253;
			// 
			// TrayNotify
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
			this.CancelButton = this.btChiudi;
			this.ClientSize = new System.Drawing.Size(300, 80);
			this.Controls.Add(this.lkLog);
			this.Controls.Add(this.btChiudi);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(label1);
			this.Controls.Add(this.dgLog);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TrayNotify";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Notifiche WS8610:";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.TrayNotify_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgLog)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button btChiudi;
		private System.Windows.Forms.LinkLabel lkLog;
		private System.Windows.Forms.DataGridView dgLog;
		private System.Windows.Forms.DataGridViewTextBoxColumn ora;
		private System.Windows.Forms.DataGridViewTextBoxColumn msg;
	}
}