using System;

namespace WS8610
{
	public class HistoryRecord
	{
		public const double NO_TEMP = 81.0;
		public const int NO_HUM = 110;

		public int Index;
		public DateTime DateTime;
		public double[] Temp;
		public int[] Hum;
		public bool IsLast;
		public bool IsValid;

		public string TempStr(int i) {
			return (Temp.Length <= i || Temp[i] == NO_TEMP) ? "--" : Temp[i].ToString("0.0");
		}

		public string HumStr(int i) {
			return (Hum.Length <= i || Hum[i] == NO_HUM) ? "--" : Hum[i].ToString();
		}

		public string DateTimeStr => DateTime.ToString("dd/MM/yyyy HH:mm");

	    public HistoryRecord(int index) {
			Index = index;
			IsValid = false;
		}
	}
}