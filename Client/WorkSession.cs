using System;
using System.Linq;
using HttpMock.Core;

namespace HttpMock.Client
{
	public class WorkSession
	{
		public ConnectionSettings ConnectionSettings { get; set; }

		public Mock[] Mocks { get; set; }

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
				Mocks = loadedWorkSession.Mocks?.Where(mock => mock != null).ToArray();
			}

			ConnectionSettings ??= new ConnectionSettings();

			Mocks ??= Array.Empty<Mock>();
		}
	}
}