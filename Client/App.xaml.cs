using System.Linq;
using System.Windows;
using TcpMock.Core;

namespace TcpMock.Client
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly WorkSessionSaver _saver = new WorkSessionSaver(new EnvironmentWrapper());

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			WorkSession workSession = _saver.Load();

			ConnectionSettingsCache.Init(workSession.ConnectionSettings);
			MockCache.Init(workSession.Mocks);
		}

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);

			var workSession = new WorkSession
			{
				ConnectionSettings = ConnectionSettingsCache.ConnectionSettings,
				Mocks = MockCache.GetAll().ToArray()
			};

			_saver.Save(workSession);
		}
	}
}