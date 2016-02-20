using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WS8610;

namespace WS8610GUI
{
	public partial class Tabella : Form
	{
	    private readonly WS8610Com _wsCom;

	    public Tabella(IEnumerable<HistoryRecord> recList, WS8610Com wsCom) {
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
            if (col.StartsWith("T.") && col.Length == 3) {
                var sensorNr = Convert.ToInt32(col.Substring(2));
                var recordNr = Convert.ToInt32(dgDati.Rows[e.RowIndex].Cells[0].Value.ToString().Substring(1));

            }
        }
    }
}
