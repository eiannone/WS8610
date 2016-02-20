using System.Collections;
using System.Diagnostics;
using System.ServiceProcess;

namespace WS8610Service
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		private static void Main(string[] args) {
			if (args.Length > 0 && args[0] == "-install") {
				// Chiamata da commandline per l'installazione del servizio
				var service_path = Process.GetCurrentProcess().MainModule.FileName;
				var p_args = "create \"" + WS8610Service.SERVICE_NAME + "\" binPath= \"" + service_path + "\" start= auto";
				var pStartInfo = new ProcessStartInfo("sc", p_args);
				new Process { StartInfo = pStartInfo }.Start();
				pStartInfo.Arguments = "start \"" + WS8610Service.SERVICE_NAME + "\"";
				new Process { StartInfo = pStartInfo }.Start();
			}
			else if (args.Length > 0 && args[0] == "-uninstall") {
				// Chiamata da commandline per la disinstallazione del servizio
				var service_path = Process.GetCurrentProcess().MainModule.FileName;
				var p_args = "stop \"" + WS8610Service.SERVICE_NAME + "\"";
				var pStartInfo = new ProcessStartInfo("sc", p_args);
				new Process { StartInfo = pStartInfo }.Start();
				pStartInfo.Arguments = "delete \"" + WS8610Service.SERVICE_NAME + "\"";
				new Process { StartInfo = pStartInfo }.Start();
			}
			else {
				var ServicesToRun = new ServiceBase[] { new WS8610Service() };
				ServiceBase.Run(ServicesToRun);
			}
		}
	}
}