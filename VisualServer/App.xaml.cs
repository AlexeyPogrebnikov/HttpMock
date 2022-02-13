using System.Linq;
using System.Windows;
using System.Windows.Threading;
using HttpMock.Core;
using HttpMock.VisualServer.Model;
using Serilog;

namespace HttpMock.VisualServer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly EnvironmentWrapper _environmentWrapper = new();
		private readonly RouteUICollection _routes;

		public App()
		{
			ServiceLocator.Init();
			_routes = ServiceLocator.Resolve<RouteUICollection>();

			LogHelper.Init(false);
			Dispatcher.UnhandledException += Dispatcher_UnhandledException;
		}

		private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			Log.Fatal(e.Exception, "An unhandled exception occurred");
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var workSession = new WorkSession();
			workSession.Load(_environmentWrapper);

			ConnectionSettingsCache.Init(workSession.ConnectionSettings);
			_routes.Init(workSession.Routes.Select(route => route.Convert()));
		}

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);

			var httpServer = ServiceLocator.Resolve<IVisualHttpServer>();
			var workSession = new WorkSession
			{
				ConnectionSettings = ConnectionSettingsCache.ConnectionSettings,
				Routes = httpServer.Routes.ToArray()
			};

			workSession.Save(_environmentWrapper);
		}
	}
}