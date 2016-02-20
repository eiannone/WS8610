using System;
using System.Collections.Generic;

namespace WS8610Recorder
{
	public class DailyMeasurements
	{
		public readonly List<Dictionary<TimeSpan, decimal>> Temperature;
		public readonly List<Dictionary<TimeSpan, int>> Humidity;

		public DailyMeasurements(int num_sensors) {
			Temperature = new List<Dictionary<TimeSpan, decimal>>();
			Humidity = new List<Dictionary<TimeSpan, int>>();
			for (var s = 0; s < num_sensors; s++) {
				Temperature.Add(new Dictionary<TimeSpan, decimal>());
				Humidity.Add(new Dictionary<TimeSpan, int>());
			}
		}
	}
}
