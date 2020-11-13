using System.Linq;
using System.Windows;
using HttpMock.Core;

namespace HttpMock.Client
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly WorkSessionSaver _saver = new WorkSessionSaver(new EnvironmentWrapper());
		private readonly IMockCache _mockCache;

		public App()
		{
			ServiceLocator.Init();
			_mockCache = ServiceLocator.Resolve<IMockCache>();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			WorkSession workSession = _saver.Load();

			ConnectionSettingsCache.Init(workSession.ConnectionSettings);
			_mockCache.Init(workSession.Mocks);
		}

		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);

			var workSession = new WorkSession
			{
				ConnectionSettings = ConnectionSettingsCache.ConnectionSettings,
				Mocks = _mockCache.GetAll().ToArray()
			};

			_saver.Save(workSession);
		}
	}
}