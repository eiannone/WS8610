using System;

namespace WS8610Recorder
{
	public struct SensorMeasure { public decimal? Temp; public int? Humid; }

	public class Measure
	{
		public DateTime DateTime { get; set; }
		public string DateTimeStr => DateTime.ToString("dd/MM/yyyy HH:mm");
	    public readonly SensorMeasure[] Sensor = new SensorMeasure[4];

		private bool? _daylightSaving;
		public bool IsDaylightSaving {
			get {
				if (_daylightSaving == null) return DateTime.IsDaylightSavingTime();
				return (bool)(_daylightSaving);
			}
			set { _daylightSaving = value; }
		}

		public int MinutesId {
			get {
				var idMin = 1 + (int)(DateTime.TimeOfDay.TotalMinutes / 5) * 2;
				return IsDaylightSaving ? idMin + 1 : idMin;
			}
		}

		public Measure(DateTime dt) {
			DateTime = dt;
		}
	}
}
