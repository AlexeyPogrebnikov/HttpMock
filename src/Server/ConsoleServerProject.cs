using System.Collections.Generic;
using System.Net;
using HttpMock.Server.Core;

namespace HttpMock.Server
{
	internal class ConsoleServerProject
	{
		public ConsoleServerProject(ConsoleArgs consoleArgs)
		{
			if (!string.IsNullOrEmpty(consoleArgs.ServerProjectFileName))
			{
				var serverProject = new ServerProject();
				serverProject.Load(consoleArgs.ServerProjectFileName);

				Address = IPAddress.Parse(serverProject.Connection.Host);
				Port = serverProject.Connection.Port;
				Routes = serverProject.Routes;
			}

			if (consoleArgs.Host != null)
				Address = consoleArgs.Host;

			if (consoleArgs.Port.HasValue)
				Port = consoleArgs.Port.Value;
		}

		internal IPAddress Address { get; }
		internal int Port { get; }
		internal IEnumerable<Route> Routes { get; }
	}
}