using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;


namespace WS8610Service
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : Installer
	{
		public ProjectInstaller() {
			InitializeComponent();
		}

		private static void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e) {
			var sc = new ServiceController(WS8610Service.SERVICE_NAME);
			sc.Start();
		}
	}
}
