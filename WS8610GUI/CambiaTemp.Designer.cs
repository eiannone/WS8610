namespace WS8610GUI
{
    partial class CambiaTemp
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
            System.Windows.Forms.Label label1;
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btSalva = new System.Windows.Forms.Button();
            this.btAnnulla = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(13, 13);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(99, 13);
            label1.TabIndex = 0;
            label1.Text = "Valore temperatura:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(144, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(58, 20);
            this.textBox1.TabIndex = 1;
            // 
            // btSalva
            // 
            this.btSalva.Location = new System.Drawing.Point(87, 42);
            this.btSalva.Name = "btSalva";
            this.btSalva.Size = new System.Drawing.Size(75, 23);
            this.btSalva.TabIndex = 2;
            this.btSalva.Text = "Salva";
            this.btSalva.UseVisualStyleBackColor = true;
            // 
            // btAnnulla
            // 
            this.btAnnulla.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btAnnulla.Location = new System.Drawing.Point(169, 42);
            this.btAnnulla.Name = "btAnnulla";
            this.btAnnulla.Size = new System.Drawing.Size(75, 23);
            this.btAnnulla.TabIndex = 3;
            this.btAnnulla.Text = "Annulla";
            this.btAnnulla.UseVisualStyleBackColor = true;
            // 
            // CambiaTemp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btAnnulla;
            this.ClientSize = new System.Drawing.Size(256, 77);
            this.Controls.Add(this.btAnnulla);
            this.Controls.Add(this.btSalva);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CambiaTemp";
            this.ShowIcon = false;
            this.Text = "Modifica temperatura";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btSalva;
        private System.Windows.Forms.Button btAnnulla;
    }
}