using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using WS8610;

namespace WS8610GUI
{
	public partial class Tabella : Form
	{
	    private readonly WS8610Com _wsCom;

	    public Tabella(IEnumerable<HistoryRecord> recList, WS8610Com wsCom)
        {
	        _wsCom = wsCom;
	        InitializeComponent();

			foreach(var hr in recList.Reverse()) {
				dgDati.Rows.Add("#" + hr.Index.ToString("0000"),
					hr.DateTime.ToString("dd/MM/yyyy HH:mm"), 
					hr.TempStr(0), hr.HumStr(0),
					hr.TempStr(1), hr.HumStr(1),
					hr.TempStr(2), hr.HumStr(2),
					hr.TempStr(3), hr.HumStr(3));
			}
		}

        private void dgDati_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var col = dgDati.Columns[e.ColumnIndex].HeaderText;
            if (col.Length != 3 || (!col.StartsWith("T.") && !col.StartsWith("U."))) return;

            var tipo = col.StartsWith("T") ? CambiaVal.TipoVal.Temperatura : CambiaVal.TipoVal.Umidita;
            var sensorNr = Convert.ToInt32(col.Substring(2));
            var cells = dgDati.Rows[e.RowIndex].Cells;
            var recordNr = Convert.ToInt32(cells[0].Value.ToString().Substring(1));
            var valStr = (string)cells[e.ColumnIndex].Value;
            double? val = (valStr == "--")? null : (double?)Convert.ToDouble(valStr);

            var win = new CambiaVal(recordNr, sensorNr, val, tipo);
            var res = win.ShowDialog();
            if (res != DialogResult.OK) return;

            var wasOpen = _wsCom.IsOpen;
            if (!wasOpen) _wsCom.Open();
            if (tipo == CambiaVal.TipoVal.Temperatura) {
                _wsCom.SetTemp(win.NRecord, win.NSensore, 
                    (win.Val == null)? HistoryRecord.NO_TEMP : (double)win.Val);
            } else {
                _wsCom.SetHumidity(win.NRecord, win.NSensore, 
                    (win.Val == null)? HistoryRecord.NO_HUM : Convert.ToInt32(win.Val));
            }
            if (!wasOpen) _wsCom.Close();
            if (recordNr == win.NRecord) {
                var colIndex = 2 + (win.NSensore*2);
                if (tipo == CambiaVal.TipoVal.Umidita) colIndex++;
                cells[colIndex].Value = (win.Val == null)? "--" : ((double)win.Val).ToString(CultureInfo.CurrentCulture);
            }
        }
    }
}
