using System.Net;
using HttpMock.Core;

namespace HttpMock.ConsoleClient
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			ConsoleArgs consoleArgs = ConsoleArgs.Parse(args);

			/*if (!consoleArgs.Validate())
				return;*/

			ServerProject serverProject = consoleArgs.CreateServerProject();

			var mockCache = new MockCache();
			mockCache.AddRange(serverProject.Mocks);

			IHttpServer httpServer = new HttpServer(mockCache, new HttpInteractionCache());

			IPAddress address = IPAddress.Parse(serverProject.Host);
			int port = int.Parse(serverProject.Port);
			httpServer.Start(address, port);
		}
	}
}