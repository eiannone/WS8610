using System;
using System.Globalization;
using System.Windows.Forms;
using WS8610;

namespace WS8610GUI
{
    public sealed partial class CambiaVal : Form
    {
        public enum TipoVal { Temperatura, Umidita }

        public int NRecord { get; private set; }
        public int NSensore { get; private set; }
        public double Val { get; private set; }

        public CambiaVal(int nRecord, int nSensore, double val, TipoVal tipo)
        {
            InitializeComponent();
            if (tipo == TipoVal.Temperatura) {
                Text = @"Modifica temperatura";
                lbVal.Text = @"Val. temperatura:";
            }
            else {
                Text = @"Modifica umidità";
                lbVal.Text = @"Val. umidità:";
            }

            tbRecord.Text = nRecord.ToString();
            cbSensore.SelectedIndex = nSensore;
            tbVal.Text = val.ToString(CultureInfo.CurrentCulture);
        }

        private void btSalva_Click(object sender, EventArgs e)
        {
            try {
                NRecord = Convert.ToInt32(tbRecord.Text);
                NSensore = cbSensore.SelectedIndex;
                Val = Convert.ToDouble(tbVal.Text);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch(Exception ex) {
                MessageBox.Show(@"Errore: " + ex.Message, @"Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
