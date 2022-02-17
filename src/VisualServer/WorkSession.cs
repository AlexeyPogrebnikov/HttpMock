using System;
using System.Linq;
using HttpMock.Core;

namespace HttpMock.VisualServer
{
	public class WorkSession
	{
		public ConnectionSettings ConnectionSettings { get; set; }

		public Route[] Routes { get; set; }

		public void Save(IEnvironmentWrapper environmentWrapper)
		{
			var saver = new WorkSessionSaver(environmentWrapper);
			saver.Save(this);
		}

		public void Load(IEnvironmentWrapper environmentWrapper)
		{
			var saver = new WorkSessionSaver(environmentWrapper);
			WorkSession loadedWorkSession = saver.Load();
			if (loadedWorkSession != null)
			{
				ConnectionSettings = loadedWorkSession.ConnectionSettings;
				Routes = loadedWorkSession.Routes?.Where(route => route != null).ToArray();
			}

			ConnectionSettings ??= new ConnectionSettings();

			Routes ??= Array.Empty<Route>();
		}
	}
}