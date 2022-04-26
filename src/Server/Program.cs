using HttpMock.Server.Core;
using Serilog;

namespace HttpMock.Server
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			LogHelper.Init(true);

			Log.Information($"Version: {VersionHelper.GetCurrentAppVersion()}");

			ConsoleArgs consoleArgs = new(args);

			ConsoleServerProject project = new(consoleArgs);
			ConsoleHttpServer httpServer = new(project.Routes);
			httpServer.Start(project.Address, project.Port);
		}
	}
}