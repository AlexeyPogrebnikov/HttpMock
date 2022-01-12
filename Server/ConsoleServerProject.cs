using HttpMock.Core;
using System.Net;

namespace HttpMock.Server
{
	internal class ConsoleServerProject
	{
		private readonly ServerProject _serverProject;
		private readonly IHttpServer _httpServer;
		private readonly IPAddress host;
		private readonly int port;

		public ConsoleServerProject(ConsoleArgs consoleArgs, IHttpServer httpServer)
		{
			_httpServer = httpServer;

			if (!string.IsNullOrEmpty(consoleArgs.ServerProjectFileName))
			{
				_serverProject = new ServerProject();
				_serverProject.Load(consoleArgs.ServerProjectFileName);
				
				httpServer.Routes.Init(_serverProject.Routes);

				host = IPAddress.Parse(_serverProject.Connection.Host);
				port = _serverProject.Connection.Port;
			} 

			if (consoleArgs.Host != null)
				host = consoleArgs.Host;

			if (consoleArgs.Port.HasValue)
				port = consoleArgs.Port.Value;
		}

		internal void StartServer()
		{
			_httpServer.Start(host, port);
		}
	}
}
