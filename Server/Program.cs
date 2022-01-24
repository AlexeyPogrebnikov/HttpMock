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
			_httpServer.Interactions.ItemAdded += Requests_ItemAdded;

			ConsoleServerProject project = new(consoleArgs, _httpServer);

			project.StartServer();
		}

		private static void Requests_ItemAdded(object sender, EventArgs e)
		{
			IEnumerable<Interaction> interactions = _httpServer.Interactions.PopAll();

			foreach (var interaction in interactions)
			{
				ConsoleColor defaultColor = Console.ForegroundColor;
				Request request = interaction.Request;
				if (!request.Handled)
					Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(
					$"{request.Time} {request.Method} {request.Path} {interaction.Response.StatusCode}");
				Console.ForegroundColor = defaultColor;
			}
		}
	}
}