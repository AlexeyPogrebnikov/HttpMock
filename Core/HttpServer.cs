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

		public HttpServer()
		{
			Routes = new RouteCollection();
			Requests = new RequestCollection();
		}

		public RouteCollection Routes { get; }
		public RequestCollection Requests { get; }
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

			OnStatusChanged();

			try
			{
				DoStart(address, port);
			}
			catch (SocketException e)
			{
				if (e.SocketErrorCode != SocketError.Interrupted)
					throw;
				//TODO log error
			}
			/*catch (Exception)
			{
				//TODO log error
				throw;
			}*/
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
			catch
			{
				//TODO log error
			}
			finally
			{
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

				TimeSpan time = DateTime.Now.TimeOfDay;
				string content = GetRequestContent(stream);
				Request request = Request.Parse(content);

				var httpInteraction = new HttpInteraction
				{
					Time = time,
					Method = request.Method,
					Path = request.Path
				};

				Route route = Routes.Find(httpInteraction.Method, httpInteraction.Path).FirstOrDefault();

				httpInteraction.Handled = route != null;

				var statusCode = 404;
				if (route != null)
					statusCode = route.Response.StatusCode;

				httpInteraction.StatusCode = statusCode;

				var builder = new ResponseBuilder(Encoding.UTF8);
				builder.SetStatusCode(statusCode);
				builder.SetBody(route?.Response.Body);
				byte[] data = builder.Build();

				stream.Write(data, 0, data.Length);

				Requests.Add(httpInteraction);
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