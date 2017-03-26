using System;
using WS8610;
using WS8610Recorder;

namespace WS8610Test4
{
	class Program
	{

		static void Main(string[] args) {
            var wscom = new WS8610Com(Config.ComPort);
            try
            {
                wscom.Open();
                //var bytes = wscom.DumpMemory(0, 6);
                //foreach (var b in bytes) Console.WriteLine("{0:X}", b);

                //var rec = wscom.GetLastHistoryRecord();
                //var rec = wscom.GetHistoryRecord(0);
                //Console.WriteLine(rec.DateTimeStr);

                //var n = wscom.ExtSensors;
                //Console.WriteLine("N. sensori: " + n);
                //wscom.SetTemp(2080, 1, 13.1);
                for(var nRec = 2057; nRec < 2062; nRec++) wscom.DeleteRecord(nRec, 3);
                //var rec = wscom.GetHistoryRecords(1686);
                //Console.WriteLine("Record estratti: " + rec.Count + ". Da #" + rec[0].Index + " a #" + rec.Last().Index);
            }
            finally
            {
                if (wscom.IsOpen) wscom.Close();
            }
        }

		private static void Log(string p, LogLevel logLevel) {
			Console.WriteLine(p);
		}
	}
}
