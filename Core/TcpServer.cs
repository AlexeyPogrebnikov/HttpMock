using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpMock.Core
{
	public class TcpServer : ITcpServer
	{
		private readonly IMockCache _mockCache;
		public bool IsStarted => _server != null;

		// ReSharper disable InconsistentNaming
		private const string CRLF = "\r\n";
		// ReSharper restore InconsistentNaming

		private TcpListener _server;

		public TcpServer(IMockCache mockCache)
		{
			_mockCache = mockCache;
		}

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

					var tcpInteraction = new TcpInteraction
					{
						Uid = Guid.NewGuid(),
						Time = time,
						Method = request.Method,
						Path = request.Path
					};

					Mock mock = _mockCache.GetAll()
						.Where(m => m.Method == tcpInteraction.Method)
						.FirstOrDefault(m => m.Path == tcpInteraction.Path);

					tcpInteraction.Handled = mock != null;

					if (!IsStarted)
						return;

					var statusCode = "404";
					if (mock != null)
						statusCode = mock.StatusCode;

					tcpInteraction.StatusCode = statusCode;

					var response = $"HTTP/1.1 {statusCode} OK{CRLF}{CRLF}";

					byte[] data = Encoding.UTF8.GetBytes(response);

					stream.Write(data, 0, data.Length);

					TcpInteractionCache.Add(tcpInteraction);
				}
			}
			catch
			{
				//TODO log error
			}
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