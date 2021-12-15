using System;
using HttpMock.Core;

namespace HttpMock.ConsoleClient
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine($"Version: {VersionHelper.GetCurrentAppVersion()}");

			ConsoleArgs consoleArgs = new(args);

			MockCache mockCache = new();
			IHttpServer httpServer = new HttpServer(mockCache, new HttpInteractionCacheLogger());
			ConsoleServerProject project = new(consoleArgs, httpServer, mockCache);

			project.StartServer();
		}
	}
}