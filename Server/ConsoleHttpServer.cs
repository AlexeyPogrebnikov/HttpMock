using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using HttpMock.Core;
using Serilog;

namespace HttpMock.Server
{
	public class ConsoleHttpServer
	{
		private readonly Route _defaultRoute = new()
		{
			Response = new Response
			{
				StatusCode = 404
			}
		};

		private readonly RouteCollection _routes;

		public ConsoleHttpServer(IEnumerable<Route> routes)
		{
			_routes = new RouteCollection(routes);
		}

		public void Start(IPAddress address, int port)
		{
			try
			{
				DoStart(address, port);
			}
			catch (Exception e)
			{
				Log.Error(e, "Failed start the server or process a request.");
			}
		}

		private void DoStart(IPAddress address, int port)
		{
			var listener = new TcpListener(address, port);

			listener.Start();

			Log.Information($"HTTP server started. Host: {address}, Port: {port}.");

			while (true)
			{
				using TcpClient client = listener.AcceptTcpClient();
				using NetworkStream stream = client.GetStream();

				Request request = Request.Read(stream);

				Log.Information($"Process request {request.Method} {request.Path}");

				Route route = _routes.Find(request.Method, request.Path).FirstOrDefault();

				var handled = true;
				if (route == null)
				{
					handled = false;
					route = _defaultRoute;
				}

				Response response = route.Response;
				response.Write(stream);

				var message = $"{request.Method} {request.Path} {response.StatusCode}";
				if (handled)
					Log.Information(message);
				else
					Log.Warning(message);
			}
		}
	}
}