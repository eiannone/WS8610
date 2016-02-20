using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WS8610;

namespace WS8610GUI
{
	public partial class ShowMemory : Form
	{
		public const string FORM_NAME = "Contenuto memoria";
		private readonly TextWriter _tw;
		private readonly string _portName;

		public ShowMemory(string port_name, TextWriter tw) {
			InitializeComponent();
			Name = FORM_NAME;
			_tw = tw;
			_portName = port_name;
		}

		private void btLeggi_Click(object sender, EventArgs e) {
			if (!Regex.IsMatch(tbIndir.Text, @"[\dA-Z]{4}")) {
				errore("Indirizzo non valido.");
				tbIndir.Focus();
				return;
			}
			var start_addr = Convert.ToInt16(tbIndir.Text, 16);
			var num_bytes = Int16.Parse(tbBytes.Text);
			if (start_addr > 0x7FFF) start_addr = 0x7FFF;
			if (start_addr + num_bytes - 1 > 0x7FFF) num_bytes = (short)(0x7FFF - start_addr + 1);
			btLeggi.Enabled = false;
			var wscom = new WS8610Com(_portName, _tw);
			wscom.Open();
			var bytes = wscom.DumpMemory(start_addr, num_bytes);
			wscom.Close();
			show_bytes(bytes, start_addr);
			btLeggi.Enabled = true;
		}

		private void show_bytes(IEnumerable<byte> bytes, short start_addr) {
			flowLayoutPanel.Controls.Clear();
			var addr = start_addr;
			foreach (var addrval in bytes.Select(b => new HexByte(b, addr++))) {
				flowLayoutPanel.Controls.Add(addrval);
			}
		}

		private void errore(string msg) {
			MessageBox.Show(this, msg, @"Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void tbIndir_KeyUp(object sender, KeyEventArgs e) {
			tbIndir.Text = tbIndir.Text.ToUpper();
		}

		private void tbIndir_Leave(object sender, EventArgs e) {
			tbIndir.Text = tbIndir.Text.PadLeft(4, '0');
		}

		private void bt08_Click(object sender, EventArgs e) {
			read_memory(0x0008, 5);
		}

		private void read_memory(short address, int num_bytes) {
			tbIndir.Text = address.ToString("X4");
			tbBytes.Text = num_bytes.ToString();
			btLeggi_Click(null, null);
		}

		private void bt4B_Click(object sender, EventArgs e) {
			read_memory(0x004B, 6);
		}

		private void bt7FF0_Click(object sender, EventArgs e) {
			read_memory(0x7FF8, 8);
		}
	}
}