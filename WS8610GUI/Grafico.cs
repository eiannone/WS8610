using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WS8610GUI.Properties;
using WS8610Recorder;
using ZedGraph;

namespace WS8610GUI
{
	public partial class Grafico : Form
	{
		private readonly DaoMeasurements db = new DaoMeasurements();
		private readonly PointPairList[] temperature = new PointPairList[4];
		private readonly PointPairList[] umidita = new PointPairList[4];

		public Grafico() {
			InitializeComponent();
			cb4.Text = Settings.Default.Sensor4Name;

			var g = zg.GraphPane;
			g.Title.IsVisible = false;
			g.Border.IsVisible = false;
			g.Fill.Color = SystemColors.Control;
			g.Legend.IsVisible = false;
			g.Chart.Fill = new Fill( Color.White, Color.Gainsboro, 45.0f );

			// Asse X - tempo
			g.XAxis.Title.IsVisible = false;
			g.XAxis.Type = AxisType.Date;
			g.XAxis.MajorGrid.IsVisible = true;
			g.XAxis.Scale.MinGrace = g.XAxis.Scale.MaxGrace = 0.01;

			// Asse Temperatura
			g.YAxis.Title.Text = "Temperatura °C";
			g.YAxis.Title.FontSpec.IsBold = false;
			g.YAxis.MajorGrid.IsVisible = true;
			g.YAxis.MajorTic.IsOpposite = false;
			g.YAxis.MinorTic.IsOpposite = false;

			// Asse Umidità
			g.Y2Axis.IsVisible = cbUmid.Checked;
			g.Y2Axis.Title.Text = "Umidità %";
			g.Y2Axis.Title.FontSpec.IsBold = false;
			g.Y2Axis.Title.FontSpec.FontColor = g.Y2Axis.Scale.FontSpec.FontColor = Color.DimGray;
			g.Y2Axis.MajorTic.IsOpposite = false;
			g.Y2Axis.MinorTic.IsOpposite = false;

			zg.PointValueEvent += zg_PointValueEvent;

			var last_record = db.GetLastRecord();
			dtA.Value = last_record.DateTime;
			dtDa.Value = dtA.Value.AddDays(-2);
			btAggiorna_Click(null, null);
		}

		static string zg_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt) {
			var pt = curve[iPt];
			var dataora = XDate.XLDateToDateTime(pt.X).ToString("dd/MM/yyyy HH:mm");
			var valore = curve.IsY2Axis ? pt.Y + "%" : pt.Y.ToString("f1") + " °C";
			return "Valore: " + valore + Environment.NewLine + "Data: " + dataora + Environment.NewLine + 
				"(" + curve.Label.Text + ")";
		}

		private void cb_CheckedChanged(object sender, EventArgs e) {
			var cb = (CheckBox) sender;
			var style = cb.Checked ? FontStyle.Underline | FontStyle.Bold : FontStyle.Regular;
			cb.Font = new Font(cb.Font, style);
			aggiorna_grafico();
		}

		private void btAggiorna_Click(object sender, EventArgs e) {
			for (var i = 0; i < 4; i++) {
				temperature[i] = new PointPairList();
				umidita[i] = new PointPairList();
			}

			var dati = db.GetHistoryRecords(dtDa.Value, dtA.Value);
			foreach (DataRow r in dati.Rows) {
				var dt = new XDate(((DateTime)r["giorno"]).Add(((DateTime)r["minuto"]).TimeOfDay));
				for (var i = 0; i < 4; i++) {
					if (!(r["temp" + i] is DBNull)) temperature[i].Add(dt, Convert.ToDouble(r["temp" + i]));
					if (!(r["umid" + i] is DBNull)) umidita[i].Add(dt, Convert.ToDouble(r["umid" + i]));
				}
			}
			aggiorna_grafico();
		}

		private void aggiorna_grafico() {
			zg.GraphPane.CurveList.Clear();
			if (cbBase.Checked) add_curve("cameretta", 0, Color.DarkRed);
			if (cbEsterno.Checked) add_curve("esterno", 1, Color.DarkBlue);
			if (cbCamera.Checked) add_curve("c.letto", 2, Color.DarkGreen);
			if (cb4.Checked) add_curve(cb4.Text, 3, Color.DimGray);

			zg.AxisChange();
			zg.Invalidate();

			//zg.GraphPane.XAxis.Scale.Min = XDate.DateTimeToXLDate(dtDa.Value);
			//zg.GraphPane.XAxis.Scale.Max = XDate.DateTimeToXLDate(dtA.Value);
			//zg.GraphPane.XAxis.Scale.MajorUnit = DateUnit.Hour;
			//zg.GraphPane.XAxis.Scale.MinorUnit = DateUnit.Minute;
			//zg.GraphPane.XAxis.Scale.Format = "HH:mm";
		}

		private void add_curve(string title, int id, Color color) {
			if (cbTemp.Checked && temperature[id] != null) {
				var c = new LineItem("Temperatura " + title) {
					Points = temperature[id],
					Color = color,
					Symbol = {IsVisible = false},
					Line = { Width = 2F, IsSmooth = false }
				};
				zg.GraphPane.CurveList.Add(c);
			}
			if (cbUmid.Checked && umidita[id] != null) {
				var col_light = new HSBColor(color);
				col_light.B += 20;
				var c = new LineItem("Umidità " + title) {
				  Points = umidita[id],
				  Color = col_light,
				  IsY2Axis = true,
				  Symbol = {IsVisible = false},
					Line = { Width = 1F, Style = DashStyle.Dash, IsSmooth = true, SmoothTension = 0.1f }
				};
				zg.GraphPane.CurveList.Add(c);
			}
		}

		private void cbTemp_CheckedChanged(object sender, EventArgs e) {
			zg.GraphPane.YAxis.IsVisible = cbTemp.Checked;
			aggiorna_grafico();
		}

		private void cbUmid_CheckedChanged(object sender, EventArgs e) {
			zg.GraphPane.Y2Axis.IsVisible = cbUmid.Checked;
			aggiorna_grafico();
		}


		private void btUlt12_Click(object sender, EventArgs e) {
			dtA.Value = DateTime.Now;
			dtDa.Value = dtA.Value.AddHours(-12);
			btAggiorna_Click(null, null);
		}

		private void cbUlt24_Click(object sender, EventArgs e) {
			dtA.Value = DateTime.Now;
			dtDa.Value = dtA.Value.AddHours(-24);
			btAggiorna_Click(null, null);
		}

		private void bt2gg_Click(object sender, EventArgs e) {
			dtA.Value = DateTime.Now;
			dtDa.Value = dtA.Value.AddDays(-2);
			btAggiorna_Click(null, null);
		}

		private void btSett_Click(object sender, EventArgs e) {
			dtA.Value = DateTime.Now;
			dtDa.Value = dtA.Value.AddDays(-7);
			btAggiorna_Click(null, null);
		}

		private int su_asse;
		private void zg_MouseMove(object sender, MouseEventArgs e) {
			var p = PointToClient(MousePosition);
			var bo = zg.GraphPane.Chart.Rect;
			if (p.Y < bo.Bottom + 5) {
				if (p.X > bo.Left - 5 && p.X < bo.Left + 5) {
					Cursor.Current = Cursors.Hand;
					if (su_asse != 1) {
						toolTip1.Show("Attiva/disattiva griglia", zg, p, 3000);
						su_asse = 1;
					}
					return;
				}
				if (p.X > bo.Right - 5 && p.X < bo.Right + 5 && zg.GraphPane.Y2Axis.IsVisible) {
					Cursor.Current = Cursors.Hand;
					if (su_asse != 2) {
						toolTip1.Show("Attiva/disattiva griglia", zg, p, 3000);
						su_asse = 2;
					}
					return;
				}
			}
			su_asse = 0;
		}

		private void zg_Click(object sender, EventArgs e) {
			if (su_asse == 1) {
				zg.GraphPane.YAxis.MajorGrid.IsVisible = !zg.GraphPane.YAxis.MajorGrid.IsVisible;
				zg.Invalidate();
			}
			else if (su_asse == 2) {
				zg.GraphPane.Y2Axis.MajorGrid.IsVisible = !zg.GraphPane.Y2Axis.MajorGrid.IsVisible;
				zg.Invalidate();
			}
		}
	}
}
