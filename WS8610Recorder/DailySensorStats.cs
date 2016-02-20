namespace WS8610Recorder
{
	public class DailySensorStats
	{
		private const int RANGES = 12;

		public decimal? TempMax;
		public decimal? TempMin;
		public decimal? TempAvg;

		public int? HumMax;
		public int? HumMin;
		public int? HumAvg;

		public readonly decimal?[] TempH24;

		public bool IsEmpty {
			get {
				for (var f = 0; f < RANGES; f++) if (TempH24[f] != null) return false;
				return true;
			}
		}

		public DailySensorStats() {
			TempH24 = new decimal?[RANGES];
		}
	}
}
