using HttpMock.Core;
using System.Net;

namespace HttpMock.Server
{
	internal class ConsoleServerProject
	{
		private readonly IHttpServer _httpServer;
		private readonly IPAddress _host;
		private readonly int _port;

		public ConsoleServerProject(ConsoleArgs consoleArgs, IHttpServer httpServer)
		{
			_httpServer = httpServer;

			if (!string.IsNullOrEmpty(consoleArgs.ServerProjectFileName))
			{
				var serverProject = new ServerProject();
				serverProject.Load(consoleArgs.ServerProjectFileName);
				
				httpServer.Routes.Init(serverProject.Routes);

				_host = IPAddress.Parse(serverProject.Connection.Host);
				_port = serverProject.Connection.Port;
			} 

			if (consoleArgs.Host != null)
				_host = consoleArgs.Host;

			if (consoleArgs.Port.HasValue)
				_port = consoleArgs.Port.Value;
		}

		internal void StartServer()
		{
			_httpServer.Start(_host, _port);
		}
	}
}
