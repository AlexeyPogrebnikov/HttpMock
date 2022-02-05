using System;
using System.Collections.Generic;
using HttpMock.Core;
using Serilog;

namespace HttpMock.Server
{
	internal class Program
	{
		private static HttpServer _httpServer;

		private static void Main(string[] args)
		{
			LogHelper.Init(true);

			Log.Information($"Version: {VersionHelper.GetCurrentAppVersion()}");

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
				Request request = interaction.Request;
				string message = $"{request.Method} {request.Path} {interaction.Response.StatusCode}";
				if (request.Handled)
					Log.Information(message);
				else
					Log.Warning(message);
			}
		}
	}
}