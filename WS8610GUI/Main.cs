using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using WS8610;
using WS8610GUI.Properties;
using WS8610Recorder;

namespace WS8610GUI
{
	public partial class Main : Form
	{
		private const string SERVICE_NAME = "WS8610 Service";
		private readonly Log _logWin;
		private readonly TextBox _tbSens4;
		private readonly ServiceController _wsService;
		private readonly BackgroundWorker _notifyChecker;
		private WS8610Com _wscom;

		public Main() {
			InitializeComponent();
			lbSens4.Text = Settings.Default.Sensor4Name;
			_tbSens4 = new TextBox {
				Location = lbSens4.Location,
				ForeColor = lbSens4.ForeColor,
				Text = lbSens4.Text,
				Width = 120,
				Visible = false
			};
			_tbSens4.KeyUp += tbSens4_KeyUp;
			_tbSens4.Leave += tbSens4_Accept;
			_tbSens4.LostFocus += tbSens4_Accept;
			gbSens4.Controls.Add(_tbSens4);

			// Nome della porta
			var portNames = SerialPort.GetPortNames();
			if (portNames.Length == 0) {
				tsPorte.Text = @"---";
				tsPorte.Enabled = false;
				btAggiorna.Enabled = false;
			}
			else {
				foreach (var port in portNames) tsPorte.DropDownItems.Add(port);
				tsPorte.Text = portNames[0];
			}

			lbData.Text = lbOra.Text = @"-";
			lbIndex.Text = "";
			lbUcam.Text = lbTcam.Text = lbTest.Text = lbUest.Text = lbTletto.Text = lbUletto.Text = lbTcuc.Text = lbUcuc.Text = @"--";

			// Orologio
			timer_Tick(null, null);
			timer.Start();
			_logWin = new Log();
			_logWin.Closing += delegate { tsMemoLog.Checked = false; };
			// stato del servizio WS8610 Service
			if (ServiceController.GetServices().Count(sc => sc.ServiceName == SERVICE_NAME) > 0) {
				_wsService = new ServiceController(SERVICE_NAME);
			}
			update_service_status();

			tsMemoLog.ToolTipText = @"Livello di log = " + WS8610Com.LOG_LEVEL;
			if (WS8610Com.LOG_LEVEL == 0) tsMemoLog.Enabled = false;

			tsEsci.Click += tsEsci_Click;
			tsVsLog.Click += tsServLog_Click;
			tsGrafico.Click += tsStatGrafico_Click;

			_wscom = new WS8610Com(tsPorte.Text, _logWin.Logger);

			// Inizializza il background worker per il controllo di notifiche dal servizio
			_notifyChecker = new BackgroundWorker {
				WorkerReportsProgress = false,
				WorkerSupportsCancellation = true
			};
			_notifyChecker.DoWork += check_notifies;
			_notifyChecker.RunWorkerAsync();

			SystemEvents.SessionSwitch += switch_event;
			SystemEvents.PowerModeChanged += power_event;
		}

		private void switch_event(object sender, SessionSwitchEventArgs e) {
			switch (e.Reason) {
				case SessionSwitchReason.SessionLock:
					if (_notifyChecker.IsBusy) _notifyChecker.CancelAsync();
					break;
				case SessionSwitchReason.SessionUnlock:
					if (!_notifyChecker.IsBusy) _notifyChecker.RunWorkerAsync();
					break;
			}
		}

		private void power_event(object sender, PowerModeChangedEventArgs e) {
			if (e.Mode == PowerModes.Suspend && _notifyChecker.IsBusy) _notifyChecker.CancelAsync();
		}

		private void check_notifies(object sender, DoWorkEventArgs e) {
			// Per 3 minuti controlla ogni 30 secondi se ci sono notifiche
			for (var m = 0; m < 6; m++) {
				var notifies = Recorder.GetNotifies();
				if (notifies.Length > 0) {
					Invoke((MethodInvoker)delegate {
						var win = new TrayNotify(notifies);
						win.Show();
					});
					return;
				}
				for (var w = 0; w < 5; w++) {
					Thread.Sleep(6000);
					if (_notifyChecker.CancellationPending) return;
				}
			}
		}

		private static void tsEsci_Click(object sender, EventArgs e) {
			Application.Exit();
		}

		private void tbSens4_KeyUp(object sender, KeyEventArgs e) {
			switch (e.KeyData) {
				case Keys.Escape:
					lbSens4.Show();
					_tbSens4.Hide();
					break;
				case Keys.Return:
					tbSens4_Accept(null, null);
					break;
			}
		}

		private void timer_Tick(object sender, EventArgs e) {
			tsOrario.Text = DateTime.Now.ToString("HH:mm");
			var min = DateTime.Now.Minute % 5;
			if (min == 0 || min == 4) {
				tsOrario.ForeColor = Color.Red;
				tsOrario.Font = new Font(tsOrario.Font, FontStyle.Bold);
			}
			else {
				tsOrario.ForeColor = Color.Green;
				tsOrario.Font = new Font(tsOrario.Font, FontStyle.Regular);
			}
			var secsToGo = 60 - DateTime.Now.Second;
			timer.Interval = secsToGo * 1000;
			update_service_status();
		}

		private void tsPorte_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e) {
			if (e.ClickedItem.Text == tsPorte.Text) return;
			if (_wscom.IsOpen) {
				MessageBox.Show(@"Impossibile cambiare porta perché è in uso.", @"Errore", MessageBoxButtons.OK,
												MessageBoxIcon.Error);
				return;
			}
			tsPorte.Text = e.ClickedItem.Text;
			_wscom = new WS8610Com(tsPorte.Text, _logWin.Logger);
		}

		private void btAggiorna_Click(object sender, EventArgs e) {
			btAggiorna.Enabled = false;
			try {
				if (!_wscom.Open()) throw new Exception("Connessione non riuscita.");
				var rec = _wscom.GetLastHistoryRecord();
				var memCount = _wscom.GetStoredHistoryCount();
				var maxHistory = _wscom.MaxHistoryRecords;
				_wscom.Close();
				aggiorna_pannello(rec);
				lbMem.Text = "In mem: " + memCount + "/" + maxHistory +
					"  (restanti: " + gg_hh_mm(maxHistory - memCount) + ")";
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message, @"Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally {
				if (_wscom.IsOpen) _wscom.Close();
				btAggiorna.Enabled = true;
			}
		}

		private static string gg_hh_mm(int steps) {
			var min = steps * 5;
			var m = min % 60;
			var ore = (min - m) / 60;
			var h = ore % 24;
			var g = (ore - h) / 24;

			return ((g > 0) ? g + "g " : "") + h + ":" + m.ToString("00");
		}

		private void lbSens4_Click(object sender, EventArgs e) {
			_tbSens4.Text = lbSens4.Text;
			_tbSens4.Show();
			lbSens4.Hide();
		}

		private void tbSens4_Accept(object sender, EventArgs e) {
			Settings.Default.Sensor4Name = lbSens4.Text = _tbSens4.Text;
			lbSens4.Show();
			_tbSens4.Hide();
			Settings.Default.Save();
		}

		private void Main_Click(object sender, EventArgs e) {
			if (_tbSens4.Visible) tbSens4_Accept(null, null);
		}

		private void btMostra_Click(object sender, EventArgs e) {
			btMostra.Enabled = false;
			try {
				if (!_wscom.Open()) throw new Exception("Connessione non riuscita.");
				List<HistoryRecord> recList;
				if (rbUltimi.Checked) {
					recList = _wscom.GetLastHistoryRecords(int.Parse(tbUlt.Text));
					if (recList.Count > 0) aggiorna_pannello(recList[recList.Count - 1]);
				}
				else {
					var da = int.Parse(tbDa.Text);
					var a = int.Parse(tbA.Text);
					if (da > a) throw new Exception("Intervallo di record non valido.");
					recList = _wscom.GetHistoryRecords(da, (a - da) + 1);
				}
				_wscom.Close();
				var tab = new Tabella(recList, _wscom);
				tab.Show(this);
			}
			catch (Exception ex) {
				if (_wscom.IsOpen) _wscom.Close();
				MessageBox.Show(ex.Message, @"Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally {
				btMostra.Enabled = true;
			}
		}

		private void aggiorna_pannello(HistoryRecord rec) {
			lbData.Text = rec.DateTime.ToString("dd/MM/yyyy");
			lbOra.Text = rec.DateTime.ToString("HH:mm");
			lbIndex.Text = @"# " + rec.Index;
			lbTcam.Text = rec.TempStr(0);
			lbUcam.Text = rec.HumStr(0);
			lbTest.Text = rec.TempStr(1);
			lbUest.Text = rec.HumStr(1);
			lbTletto.Text = rec.TempStr(2);
			lbUletto.Text = rec.HumStr(2);
			if (rec.Temp.Length > 3) {
				lbTcuc.Text = rec.TempStr(3);
				lbUcuc.Text = rec.HumStr(3);
				gbSens4.Show();
			}
			else {
				gbSens4.Hide();
			}
		}

		private void update_service_status() {
			if (_wsService == null) {
				tsServ.ToolTipText = @"Servizio non disponibile";
				set_service_icons(false);
				tsServ.Image = Resources.disabled;
				tsServStartStop.Enabled = false;
				return;
			}
			tsServStartStop.Enabled = true;
			var status = "Servizio ";
			switch (_wsService.Status) {
				case ServiceControllerStatus.Running:
					status += "in esecuzione";
					set_service_icons(true);
					break;
				case ServiceControllerStatus.Stopped:
					status += "non in esecuzione";
					set_service_icons(false);
					break;
				case ServiceControllerStatus.PausePending:
				case ServiceControllerStatus.Paused:
					status += "in pausa";
					set_service_icons(false);
					break;
				case ServiceControllerStatus.ContinuePending:
					status += "in fase di riavvio";
					set_service_icons(true);
					break;
				case ServiceControllerStatus.StopPending:
					status += "in fase di interruzione";
					set_service_icons(false);
					break;
			}
			tsServ.ToolTipText = status;
		}

		private void set_service_icons(bool serviceRunning) {
			tsServ.Image = serviceRunning ? Resources.running : Resources.stop;
			tsServStartStop.Text = serviceRunning ? @"Arresta" : "Avvia";
			tsServStartStop.Image = serviceRunning ? Resources.stop : Resources.running;
			tsServInfo.Enabled = serviceRunning;
		}

		private void tbUlt_Click(object sender, EventArgs e) {
			rbUltimi.Checked = true;
		}

		private void tbDa_Click(object sender, EventArgs e) {
			rbDaA.Checked = true;
		}

		private void Main_Resize(object sender, EventArgs e) {
			if (WindowState != FormWindowState.Minimized) return;
			notifyIcon.Visible = true;
			Hide();
		}

		private void notifyIcon_Click(object sender, EventArgs e) {
		    if (((MouseEventArgs) e).Button != MouseButtons.Left) return;
		    Show();
		    WindowState = FormWindowState.Normal;
		    notifyIcon.Visible = false;
		}

		private void Main_FormClosed(object sender, FormClosedEventArgs e) {
			if (_notifyChecker.IsBusy) _notifyChecker.CancelAsync();
			SystemEvents.SessionSwitch -= switch_event;
			SystemEvents.PowerModeChanged -= power_event;
		}

		private void tsMemo_Click(object sender, EventArgs e) {
			if (tsMemo.Checked) {
				var mem = new ShowMemory(tsPorte.Text, _logWin.Logger);
				mem.Show(this);
				mem.Location = new Point(Location.X + Width + 5, Location.Y);
				mem.Closed += delegate { tsMemo.Checked = false; };
			}
			else {
				foreach (var f in OwnedForms.Where(f => f.Name == ShowMemory.FORM_NAME)) f.Close();
			}
		}

		private void tsMemoWrite_Click(object sender, EventArgs e) {
			if (tsMemoWrite.Checked) {
				var mem = new WriteMemory(tsPorte.Text, _logWin.Logger);
				mem.Show(this);
				mem.Location = new Point(Location.X + Width + 5, Location.Y);
				mem.Closed += delegate { tsMemoWrite.Checked = false; };
			}
			else {
				foreach (var f in OwnedForms.Where(f => f.Name == WriteMemory.FORM_NAME)) f.Close();
			}
		}

		private void tsMemoSave_Click(object sender, EventArgs e) {
			var fileSel = new SaveFileDialog {
				AddExtension = true,
				DefaultExt = "txt",
				Filter = @"File di testo (*.txt)|*.txt|Tutti i files|*.*",
				OverwritePrompt = false,
				Title = @"Specifica il file in cui salvare il contenuto della memoria:"
			};
			var res = fileSel.ShowDialog(this);
			if (res != DialogResult.OK || string.IsNullOrEmpty(fileSel.FileName)) return;
			var progress = new Progress { Text = @"Lettura memoria WS8610" };
			_wscom.OnProgress += progress.UpdateProgress;
			progress.DoWork = delegate {
				try {
					if (!_wscom.Open()) throw new Exception("Connessione non riuscita.");
					var recBytes = _wscom.HistoryRecSize;
					var bytesToRead = (short)(_wscom.MaxHistoryRecords * recBytes);
					var crono = Stopwatch.StartNew();
					var bytes = _wscom.DumpMemory(WS8610Com.HISTORY_BASE_ADDR, bytesToRead, true);
					var elapsed = crono.Elapsed;
					_logWin.WriteLine(@"Tempo impiegato a leggere " + bytesToRead + @" bytes: " + elapsed);
					crono.Reset();
					var file = File.CreateText(fileSel.FileName);
					for (var b = 0; b < bytesToRead; b += recBytes) {
						var str = "";
						for (var i = 0; i < recBytes; i++) str += bytes[b + i].ToString("X2") + " ";
						var min = (bytes[b] >> 4) * 10 + (bytes[b] & 0x0F);
						var hour = (bytes[b + 1] >> 4) * 10 + (bytes[b + 1] & 0x0F);
						var mday = (bytes[b + 2] >> 4) * 10 + (bytes[b + 2] & 0x0F);
						var mon = (bytes[b + 3] >> 4) * 10 + (bytes[b + 3] & 0x0F);
						var year = (bytes[b + 4] >> 4) * 10 + (bytes[b + 4] & 0x0F) + 2000;
						var addr = 100 + b;
						file.WriteLine("0x" + addr.ToString("X4") + ": " + str + mday + "/" + mon + "/" + year + " " + hour.ToString("00") + ":" + min.ToString("00"));
					}
					file.WriteLine("**********");
					file.WriteLine("Tempo impiegato: " + elapsed + Environment.NewLine);
					var bytes3 = _wscom.DumpMemory(0x0000, 6);
					var str4 = "";
					for (var i = 0; i < 6; i++) str4 += bytes3[i].ToString("X2") + " ";
					file.WriteLine("0x0000: " + str4);
					var bytes2 = _wscom.DumpMemory(0x0009, 4);
					var str3 = "";
					for (var i = 0; i < 4; i++) str3 += bytes2[i].ToString("X2") + " ";
					file.WriteLine("0x0009: " + str3);
					var bytesLast = _wscom.DumpMemory(0x7FF0, 16);
					var str2 = "";
					for (var i = 0; i < 16; i++) str2 += bytesLast[i].ToString("X2") + " ";
					file.WriteLine("Ultimi 16 bytes: " + str2);
					file.Close();
				}
				catch (Exception ex) {
					if (_wscom.IsOpen) _wscom.Close();
					MessageBox.Show(ex.Message, @"Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			};
			progress.OnWorkCompleted = delegate {
				_wscom.OnProgress -= progress.UpdateProgress;
				progress.Close();
			};
			progress.RunAsync();
			progress.ShowDialog(this);
		}

		private void tsMemoLog_Click(object sender, EventArgs e) {
			if (tsMemoLog.Checked) {
				_logWin.Location = new Point(Location.X + Width + 5, Location.Y + 150);
				_logWin.Show(this);
				Activate();
			}
			else {
				_logWin.Hide();
			}
		}

		private void tsStatGrafico_Click(object sender, EventArgs e) {
			var gr = new Grafico();
			gr.Show(this);
		}

		private void tsStatNulli_Click(object sender, EventArgs e) {
			var win = new Nulli();
			win.Show(this);
		}

		private void tsDbImpost_Click(object sender, EventArgs e) {
			if (tsDbImpost.Checked) {
				var imp = new ImpostazioniDb();
				imp.Show(this);
				imp.Closed += delegate { tsDbImpost.Checked = false; };
			}
			else {
				foreach (var f in OwnedForms.Where(f => f.Name == "ImpostazioniDb")) f.Close();
			}
		}

		private void tsDbScarica_Click(object sender, EventArgs e) {
			if (tsDbScarica.Checked) {
				var db = new Database(_logWin.Logger);
				db.Show(this);
				db.Location = new Point(Location.X + Width + 5, Location.Y + 6);
				db.Closed += delegate { tsDbScarica.Checked = false; };
			}
			else {
				foreach (var f in OwnedForms.Where(f => f.Name == "Database")) f.Close();
			}
		}

		private void tsDbAggiorna_Click(object sender, EventArgs e) {
			var form = new AggStatistiche(_logWin.Logger);
			form.ShowDialog(this);
		}

		private void tsDbElimina_Click(object sender, EventArgs e) {
			var form = new EliminaMisure();
			form.ShowDialog(this);
		}

		private void tsServStartStop_Click(object sender, EventArgs e) {
			if (_wsService != null) {
				var start = ((ToolStripDropDownItem)sender).Text == @"Avvia";
				var dialog = new ServiceStartStop(_wsService, start ? ServiceControllerStatus.Running : ServiceControllerStatus.Stopped);
				dialog.ShowDialog(this);
				_wsService.Refresh();
			}
			update_service_status();
		}

		private void tsServLog_Click(object sender, EventArgs e) {
			var w = new LogService();
			w.Show(this);
		}

		private void tsServInfo_Click(object sender, EventArgs e) {
			if (_wsService == null) return;
			_wsService.ExecuteCommand(200);
			MessageBox.Show(this,
											@"Percorso db configurazione scritto nel file di log.", "", MessageBoxButtons.OK,
											MessageBoxIcon.Information);
			tsServLog_Click(this, null);
		}
	}
}