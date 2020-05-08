using System;

namespace WS8610
{
	public class HistoryRecord
	{
		public const double NO_TEMP = 81.0;
        public const double ERR_TEMP = -13.5;
		public const int NO_HUM = 110;
        public const int ERR_HUM = 15;
        public const string NO_VALUE = "--";
        public const string ERR_VALUE = "-e";

        public int Index;
		public DateTime DateTime;
		public double[] Temp;
		public int[] Hum;
		public bool IsLast;
		public bool IsValid;

	    public bool HasTemp(int i) {
	        return (Temp.Length > i && Temp[i] != NO_TEMP && Temp[i] != ERR_TEMP);
	    }

	    public bool HasHum(int i) {
	        return (Hum.Length > i && Hum[i] != NO_HUM && Hum[i] != ERR_HUM);
	    }

		public string TempStr(int i) {
            if (HasTemp(i)) return Temp[i].ToString("0.0");
            return (Temp.Length > i && Temp[i] == ERR_TEMP)? ERR_VALUE : NO_VALUE;
		}

		public string HumStr(int i) {
            if (HasHum(i)) return Hum[i].ToString();
            return (Hum.Length > i && Hum[i] == ERR_HUM) ? ERR_VALUE : NO_VALUE;
		}

		public string DateTimeStr => DateTime.ToString("dd/MM/yyyy HH:mm");

	    public HistoryRecord(int index) {
			Index = index;
			IsValid = false;
		}
	}
}