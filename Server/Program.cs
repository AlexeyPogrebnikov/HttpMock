using System;
using HttpMock.Core;

namespace HttpMock.Server
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine($"Version: {VersionHelper.GetCurrentAppVersion()}");

			ConsoleArgs consoleArgs = new(args);

			HttpServer httpServer = new(new HttpInteractionCacheLogger());
			ConsoleServerProject project = new(consoleArgs, httpServer);

			project.StartServer();
		}
	}
}