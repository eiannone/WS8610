namespace WS8610GUI
{
    sealed partial class CambiaVal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            this.lbVal = new System.Windows.Forms.Label();
            this.tbVal = new System.Windows.Forms.TextBox();
            this.btSalva = new System.Windows.Forms.Button();
            this.btAnnulla = new System.Windows.Forms.Button();
            this.tbRecord = new System.Windows.Forms.TextBox();
            this.cbSensore = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 14);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(57, 13);
            label2.TabIndex = 4;
            label2.Text = "Record n.:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 40);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(61, 13);
            label3.TabIndex = 5;
            label3.Text = "Sensore n.:";
            // 
            // lbVal
            // 
            this.lbVal.AutoSize = true;
            this.lbVal.Location = new System.Drawing.Point(12, 67);
            this.lbVal.Name = "lbVal";
            this.lbVal.Size = new System.Drawing.Size(87, 13);
            this.lbVal.TabIndex = 0;
            this.lbVal.Text = "Val. temperatura:";
            // 
            // tbVal
            // 
            this.tbVal.Location = new System.Drawing.Point(105, 64);
            this.tbVal.MaxLength = 4;
            this.tbVal.Name = "tbVal";
            this.tbVal.Size = new System.Drawing.Size(46, 20);
            this.tbVal.TabIndex = 1;
            // 
            // btSalva
            // 
            this.btSalva.Location = new System.Drawing.Point(86, 99);
            this.btSalva.Name = "btSalva";
            this.btSalva.Size = new System.Drawing.Size(65, 23);
            this.btSalva.TabIndex = 2;
            this.btSalva.Text = "Salva";
            this.btSalva.UseVisualStyleBackColor = true;
            this.btSalva.Click += new System.EventHandler(this.btSalva_Click);
            // 
            // btAnnulla
            // 
            this.btAnnulla.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btAnnulla.Location = new System.Drawing.Point(15, 99);
            this.btAnnulla.Name = "btAnnulla";
            this.btAnnulla.Size = new System.Drawing.Size(65, 23);
            this.btAnnulla.TabIndex = 3;
            this.btAnnulla.Text = "Annulla";
            this.btAnnulla.UseVisualStyleBackColor = true;
            // 
            // tbRecord
            // 
            this.tbRecord.Location = new System.Drawing.Point(105, 11);
            this.tbRecord.Name = "tbRecord";
            this.tbRecord.Size = new System.Drawing.Size(46, 20);
            this.tbRecord.TabIndex = 6;
            // 
            // cbSensore
            // 
            this.cbSensore.FormattingEnabled = true;
            this.cbSensore.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.cbSensore.Location = new System.Drawing.Point(105, 37);
            this.cbSensore.Name = "cbSensore";
            this.cbSensore.Size = new System.Drawing.Size(46, 21);
            this.cbSensore.TabIndex = 7;
            // 
            // CambiaVal
            // 
            this.AcceptButton = this.btSalva;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btAnnulla;
            this.ClientSize = new System.Drawing.Size(170, 138);
            this.Controls.Add(this.cbSensore);
            this.Controls.Add(this.tbRecord);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(this.btAnnulla);
            this.Controls.Add(this.btSalva);
            this.Controls.Add(this.tbVal);
            this.Controls.Add(this.lbVal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CambiaVal";
            this.ShowIcon = false;
            this.Text = "Modifica temperatura";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbVal;
        private System.Windows.Forms.Button btSalva;
        private System.Windows.Forms.Button btAnnulla;
        private System.Windows.Forms.TextBox tbRecord;
        private System.Windows.Forms.ComboBox cbSensore;
        private System.Windows.Forms.Label lbVal;
    }
}