namespace WS8610GUI
{
	partial class Nulli
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.Label label1;
			this.dgvNulli = new System.Windows.Forms.DataGridView();
			this.Data = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cbSensore = new System.Windows.Forms.ComboBox();
			label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dgvNulli)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvNulli
			// 
			this.dgvNulli.AllowUserToAddRows = false;
			this.dgvNulli.AllowUserToDeleteRows = false;
			this.dgvNulli.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvNulli.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgvNulli.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
			this.dgvNulli.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvNulli.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Data});
			this.dgvNulli.Location = new System.Drawing.Point(0, 26);
			this.dgvNulli.Name = "dgvNulli";
			this.dgvNulli.ReadOnly = true;
			this.dgvNulli.RowHeadersVisible = false;
			this.dgvNulli.Size = new System.Drawing.Size(562, 467);
			this.dgvNulli.TabIndex = 0;
			this.dgvNulli.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvNulli_CellPainting);
			// 
			// Data
			// 
			dataGridViewCellStyle10.BackColor = System.Drawing.Color.WhiteSmoke;
			dataGridViewCellStyle10.Format = "dd/MM/yyyy - ddd";
			this.Data.DefaultCellStyle = dataGridViewCellStyle10;
			this.Data.Frozen = true;
			this.Data.HeaderText = "Data";
			this.Data.Name = "Data";
			this.Data.ReadOnly = true;
			this.Data.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(6, 7);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(49, 13);
			label1.TabIndex = 1;
			label1.Text = "Sensore:";
			// 
			// cbSensore
			// 
			this.cbSensore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSensore.FormattingEnabled = true;
			this.cbSensore.IntegralHeight = false;
			this.cbSensore.Items.AddRange(new object[] {
            "S.1 - Esterno SUD",
            "S.2 - Camera",
            "S.3 - Esterno NORD"});
			this.cbSensore.Location = new System.Drawing.Point(57, 2);
			this.cbSensore.Name = "cbSensore";
			this.cbSensore.Size = new System.Drawing.Size(121, 21);
			this.cbSensore.TabIndex = 2;
			this.cbSensore.SelectedIndexChanged += new System.EventHandler(this.cbSensore_SelectedIndexChanged);
			// 
			// Nulli
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(562, 493);
			this.Controls.Add(this.cbSensore);
			this.Controls.Add(label1);
			this.Controls.Add(this.dgvNulli);
			this.Name = "Nulli";
			this.Text = "Nulli";
			this.Load += new System.EventHandler(this.Nulli_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvNulli)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvNulli;
		private System.Windows.Forms.DataGridViewTextBoxColumn Data;
		private System.Windows.Forms.ComboBox cbSensore;
	}
}