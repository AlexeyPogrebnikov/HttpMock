using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpMock.Core
{
	public class HttpServer : IHttpServer
	{
		private TcpListener _listener;
		private readonly object _syncRoot = new();
		private readonly Route _defaultRoute = new()
		{
			Response = new Response
			{
				StatusCode = 404
			}
		};

		public HttpServer()
		{
			Routes = new RouteCollection();
			Interactions = new InteractionCollection();
		}

		public RouteCollection Routes { get; }
		public InteractionCollection Interactions { get; }
		public event EventHandler StatusChanged;
		public bool IsStarted { get; private set; }

		public void Start(IPAddress address, int port)
		{
			if (IsStarted == false)
			{
				lock (_syncRoot)
				{
					if (IsStarted)
						ThrowHttpServerIsAlreadyStarted();

					IsStarted = true;
				}
			}
			else
				ThrowHttpServerIsAlreadyStarted();

			Log.Information("HTTP server started.");

			OnStatusChanged();

			try
			{
				DoStart(address, port);
			}
			catch (SocketException e)
			{
				if (e.SocketErrorCode != SocketError.Interrupted)
				{
					Log.Error(e, "Failed start the server or process a request.");
					throw;
				}
			}
			catch (Exception e)
			{
				Log.Error(e, "Failed start the server or process a request.");
				throw;
			}
			finally
			{
				IsStarted = false;
				OnStatusChanged();
			}
		}

		public void Stop()
		{
			try
			{
				_listener?.Stop();
			}
			catch (Exception e)
			{
				Log.Error(e, "Failed stop the server.");
			}
			finally
			{
				Log.Information("HTTP server stopped.");

				_listener = null;
			}
		}

		private void DoStart(IPAddress address, int port)
		{
			_listener = new TcpListener(address, port);

			_listener?.Start();

			while (true)
			{
				if (_listener == null)
					return;

				using TcpClient client = _listener?.AcceptTcpClient();
				using NetworkStream stream = client?.GetStream();

				if (stream == null)
					return;

				string content = GetRequestContent(stream);
				Request request = Request.Parse(content);

				Log.Information($"Process request {request.Method} {request.Path}");

				Route route = Routes.Find(request.Method, request.Path).FirstOrDefault();

				if (route == null)
				{
					request.Handled = false;
					route = _defaultRoute;
				}

				Response response = route.Response;
				response.Write(stream);

				Interaction interaction = new()
				{
					Request = request,
					Response = response
				};

				Interactions.Add(interaction);
			}
		}

		private static string GetRequestContent(NetworkStream networkStream)
		{
			byte[] data = new byte[1024];
			using MemoryStream memoryStream = new();

			do
			{
				networkStream.Read(data);
				memoryStream.Write(data);
			} while (networkStream.DataAvailable);

			return Encoding.Default.GetString(memoryStream.ToArray());
		}

		private static void ThrowHttpServerIsAlreadyStarted()
		{
			throw new InvalidOperationException("HTTP server is already started.");
		}

		private void OnStatusChanged()
		{
			StatusChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}