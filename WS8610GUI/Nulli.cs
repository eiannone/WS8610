using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WS8610Recorder;

namespace WS8610GUI
{
	public partial class Nulli : Form
	{
		private readonly DataGridViewCellStyle _headerstyle = new DataGridViewCellStyle {
			Font = new Font("Microsoft Sans Serif", 7.6f)
		};
		private readonly DataGridViewCellStyle _cellStyle = new DataGridViewCellStyle {
			Alignment = DataGridViewContentAlignment.MiddleCenter,
			Font = new Font("Microsoft Sans Serif", 6.5f)
		};
		private readonly Color _gray = Color.FromArgb(240, 240, 240);

		public Nulli() {
			InitializeComponent();
			for (var h = 0; h < 24; h++) {
				var col = new DataGridViewTextBoxColumn {
					Name = "h" + h,
					Width = 18,
					DefaultCellStyle = _cellStyle,
					HeaderCell = new DataGridViewColumnHeaderCell { Style = _headerstyle, Value = h.ToString() }
				};
				dgvNulli.Columns.Add(col);
			}
			cbSensore.SelectedIndex = 0;
			//aggiorna_tabella();
		}

		private void dgvNulli_CellPainting(object sender, DataGridViewCellPaintingEventArgs e) {
			if (e.RowIndex == -1 && e.ColumnIndex > 0) {
				e.PaintBackground(e.ClipBounds, true);
				e.Graphics.DrawString(Convert.ToString(e.FormattedValue), e.CellStyle.Font, Brushes.DimGray,
					e.CellBounds.X + 1, e.CellBounds.Y + 2);
				e.Handled = true;
			}
			else if (e.RowIndex > -1) {
				if (((DateTime)dgvNulli.Rows[e.RowIndex].Cells[0].Value).DayOfWeek == DayOfWeek.Sunday) {
					var pen = new Pen(Color.DimGray, 2);
					e.Graphics.DrawLine(pen, e.CellBounds.Left, e.CellBounds.Top - 1, e.CellBounds.Right, e.CellBounds.Top - 1);
				}
			}
		}

		private void aggiorna_tabella() {
			dgvNulli.Rows.Clear();
			var dao = new DaoMeasurements();
			var dati = dao.GetNullTemps(cbSensore.SelectedIndex + 1, DateTime.Now.AddDays(-30), DateTime.Now).Reverse();
			foreach (var n in dati) {
				var r = dgvNulli.Rows.Add();
				dgvNulli.Rows[r].Cells[0].Value = n.Key;
				for (var c = 0; c < 24; c++) {
					var cell = dgvNulli.Rows[r].Cells[c + 1];
					if (n.Value[c] > 0) {
						cell.Value = n.Value[c];
						cell.ToolTipText = n.Value[c] + "%";
						var bc = colore_ora(n.Value[c]);
						cell.Style.BackColor = bc;
						cell.Style.ForeColor = Color.FromArgb(bc.R - 50, (bc.G > 50) ? bc.G - 50 : 0, (bc.B > 50) ? bc.B - 50 : 0);
					}
					else if (c > 17 || (c > 5 && c < 12)) cell.Style.BackColor = _gray;
				}
			}
		}

		private static Color colore_ora(int val) {
			if (val > 92) return Color.FromArgb(88, 12, 12);
			if (val > 84) return Color.FromArgb(102, 31, 31);
			if (val > 75) return Color.FromArgb(116, 50, 50);
			if (val > 67) return Color.FromArgb(130, 69, 69);
			if (val > 59) return Color.FromArgb(144, 88, 88);
			if (val > 50) return Color.FromArgb(158, 107, 107);
			if (val > 42) return Color.FromArgb(172, 126, 125);
			if (val > 34) return Color.FromArgb(186, 145, 144);
			if (val > 25) return Color.FromArgb(200, 164, 163);
			if (val > 17) return Color.FromArgb(214, 183, 182);
			if (val > 9) return Color.FromArgb(228, 202, 201);
			return Color.FromArgb(242, 220, 219);
		}

		private void Nulli_Load(object sender, EventArgs e) {
			dgvNulli.ClearSelection();
		}

		private void cbSensore_SelectedIndexChanged(object sender, EventArgs e) {
			if (cbSensore.SelectedIndex >= 0) aggiorna_tabella();
		}
	}
}