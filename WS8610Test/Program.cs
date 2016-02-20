using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
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
                var n = wscom.ExtSensors;
                Console.WriteLine("N. sensori: " + n);
                wscom.SetTemp(1050, 1, 17.4);
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
