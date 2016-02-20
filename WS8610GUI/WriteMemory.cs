using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WS8610;

namespace WS8610GUI
{
	public partial class WriteMemory : Form
	{
		public const string FORM_NAME = "Modifica memoria";
		private readonly TextWriter _tw;
		private readonly string _portName;

		public WriteMemory(string port_name, TextWriter tw) {
			InitializeComponent();
			Name = FORM_NAME;
			_tw = tw;
			_portName = port_name;
		}

		private void errore(string msg) {
			MessageBox.Show(this, msg, @"Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void btScrivi_Click(object sender, EventArgs e) {
			if (!Regex.IsMatch(tbIndir.Text, @"[\dA-Z]{4}")) {
				errore("Indirizzo non valido.");
				tbIndir.Focus();
				return;
			}
			var bytes = tbBytes.Text.Replace(" ", "");
			if (string.IsNullOrEmpty(bytes)) {
				errore("Nessun byte da scrivere");
				tbBytes.Focus();
				return;
			}
			var start_addr = Convert.ToInt16(tbIndir.Text, 16);
			btScrivi.Enabled = false;
			var wscom = new WS8610Com(_portName, _tw);
			wscom.Open();
			var success = wscom.WriteMemory(start_addr, string2bytes(bytes));
			wscom.Close();
			var msg = success ? "Scrittura completata con successo" : "Scrittura fallita. Controllare log.";
			MessageBox.Show(msg, "", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
			btScrivi.Enabled = true;
		}

		private byte[] string2bytes(string str) {
			if (str.Length % 2 == 1) str += "0";
			var bytes = new byte[str.Length / 2];
			for (var i = 0; i < bytes.Length; i++) {
				bytes[i] = byte.Parse(str.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			return bytes;
		}
	}
}