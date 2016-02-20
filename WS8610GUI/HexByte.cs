using System.Windows.Forms;

namespace WS8610GUI
{
	public partial class HexByte : UserControl
	{
		public byte Value {
			set { lbByte.Text = value.ToString("X2"); }
		}
		public short Address {
			set { lbAddr.Text = value.ToString("X4"); }
		}
		public HexByte(byte value, short address) {
			InitializeComponent();
			Value = value;
			Address = address;
		}
	}
}
