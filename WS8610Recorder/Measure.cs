using System;
using WS8610;

namespace WS8610Recorder
{
	public struct SensorMeasure { public decimal? Temp; public int? Humid; }

	public class Measure
	{
		public DateTime DateTime { get; set; }
		public string DateTimeStr {
			get { return DateTime.ToString("dd/MM/yyyy HH:mm"); }
		}
		public readonly SensorMeasure[] Sensor = new SensorMeasure[4];

		private bool? _daylight_saving;
		public bool IsDaylightSaving {
			get {
				if (_daylight_saving == null) return DateTime.IsDaylightSavingTime();
				return (bool)(_daylight_saving);
			}
			set { _daylight_saving = value; }
		}

		public int MinutesId {
			get {
				var id_min = 1 + (int)(DateTime.TimeOfDay.TotalMinutes / 5) * 2;
				return IsDaylightSaving ? id_min + 1 : id_min;
			}
		}

		public Measure(DateTime dt) {
			DateTime = dt;
		}
	}
}
