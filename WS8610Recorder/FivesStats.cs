using System;
using System.Collections.Generic;

namespace WS8610Recorder
{
	public class FivesStats
	{
		public readonly int Year;
		public readonly int Index;
		public readonly List<decimal?[]> TempH24;
		public bool IsEmpty;

		public DateTime FirstDay {
			get {
				var day1ofyear = new DateTime(Year, 1, 1);
				return day1ofyear.AddDays(Index*5);
			}
		}
		public DateTime LastDay {
			get {
				if (Index > 71) return new DateTime(Year, 12, 31);
				var day1ofyear = new DateTime(Year, 1, 1);
				return day1ofyear.AddDays((Index * 5) + 4);
			}
		}

		public FivesStats(int year, int index) {
			Index = index;
			Year = year;
			TempH24 = new List<decimal?[]>(4);
			for(var s = 0; s < 4; s++) TempH24.Add(new decimal?[12]);
		}
	}
}
