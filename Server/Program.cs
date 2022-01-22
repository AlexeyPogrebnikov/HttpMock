using System;
using System.Collections.Generic;
using HttpMock.Core;

namespace HttpMock.Server
{
	internal class Program
	{
		private static HttpServer _httpServer;
		private static void Main(string[] args)
		{
			Console.WriteLine($"Version: {VersionHelper.GetCurrentAppVersion()}");

			ConsoleArgs consoleArgs = new(args);

			_httpServer = new();
			_httpServer.Requests.ItemAdded += Requests_ItemAdded;

			ConsoleServerProject project = new(consoleArgs, _httpServer);

			project.StartServer();
		}

		private static void Requests_ItemAdded(object sender, EventArgs e)
		{
			IEnumerable<HttpInteraction> requests = _httpServer.Requests.PopAll();

			foreach (var request in requests)
			{
				ConsoleColor defaultColor = Console.ForegroundColor;
				if (!request.Handled)
					Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(
					$"{request.Time} {request.Method} {request.Path} {request.StatusCode}");
				Console.ForegroundColor = defaultColor;
			}
		}
	}
}