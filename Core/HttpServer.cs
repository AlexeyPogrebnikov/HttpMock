using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpMock.Core
{
	public class HttpServer : IHttpServer
	{
		private readonly IHttpInteractionCache _httpInteractionCache;
		private readonly IMockCache _mockCache;

		private TcpListener _server;

		public HttpServer(IMockCache mockCache, IHttpInteractionCache httpInteractionCache)
		{
			_mockCache = mockCache;
			_httpInteractionCache = httpInteractionCache;
		}

		public bool IsStarted => _server != null;

		public void Start(IPAddress address, int port)
		{
			_server = null;
			var requestParser = new RequestParser();
			try
			{
				_server = new TcpListener(address, port);

				_server.Start();

				while (true)
				{
					using TcpClient client = _server.AcceptTcpClient();
					using NetworkStream stream = client.GetStream();

					if (!IsStarted)
						return;

					TimeSpan time = DateTime.Now.TimeOfDay;
					var buffer = new byte[1024];
					stream.Read(buffer, 0, 1024);
					string content = Encoding.UTF8.GetString(buffer);
					Request request = requestParser.Parse(content);

					var httpInteraction = new HttpInteraction
					{
						Uid = Guid.NewGuid(),
						Time = time,
						Method = request.Method,
						Path = request.Path
					};

					Mock mock = _mockCache.GetAll()
						.Where(m => m.Method == httpInteraction.Method)
						.FirstOrDefault(m => m.Path == httpInteraction.Path);

					httpInteraction.Handled = mock != null;

					if (!IsStarted)
						return;

					var statusCode = "404";
					if (mock != null)
						statusCode = mock.StatusCode;

					httpInteraction.StatusCode = statusCode;

					var builder = new ResponseBuilder(Encoding.UTF8);
					builder.SetStatusCode(statusCode);
					builder.SetContent(mock?.Content);
					byte[] data = builder.Build();

					stream.Write(data, 0, data.Length);

					_httpInteractionCache.Add(httpInteraction);
				}
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
				Stop();
			}
		}

		public void Stop()
		{
			try
			{
				_server?.Stop();
			}
			catch
			{
				//TODO log error
			}
			finally
			{
				_server = null;
			}
		}
	}
}