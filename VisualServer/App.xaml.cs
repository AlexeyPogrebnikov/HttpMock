using System.Linq;
using System.Windows;
using HttpMock.Core;

namespace HttpMock.VisualServer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly EnvironmentWrapper _environmentWrapper = new EnvironmentWrapper();
		private readonly IHttpServer _httpServer;

		public App()
		{
			ServiceLocator.Init();
			_httpServer = ServiceLocator.Resolve<IHttpServer>();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			LogHelper.Init(false);

			var workSession = new WorkSession();
			workSession.Load(_environmentWrapper);

			ConnectionSettingsCache.Init(workSession.ConnectionSettings);
			_httpServer.Routes.Init(workSession.Routes);
		}

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);

			var workSession = new WorkSession
			{
				ConnectionSettings = ConnectionSettingsCache.ConnectionSettings,
				Routes = _httpServer.Routes.ToArray()
			};

			workSession.Save(_environmentWrapper);
		}
	}
}