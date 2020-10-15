using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpMock.Core
{
	public static class TcpServer
	{
		public static bool IsStarted => _server != null;

		// ReSharper disable InconsistentNaming
		private const string CRLF = "\r\n";
		// ReSharper restore InconsistentNaming

		private static TcpListener _server;

		public static void Start(IPAddress address, int port)
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

					var buffer = new byte[1024];
					stream.Read(buffer, 0, 1024);
					string content = Encoding.UTF8.GetString(buffer);
					Request request = requestParser.Parse(content);
					request.Time = DateTime.Now.TimeOfDay;
					Mock mock = MockCache.GetAll()
						.Where(m => m.Method == request.Method)
						.FirstOrDefault(m => m.Path == request.Path);

					request.Handled = mock != null;

					if (!IsStarted)
						return;

					RequestCache.Add(request);

					var statusCode = "404";
					if (mock != null)
						statusCode = mock.StatusCode;

					var response = $"HTTP/1.1 {statusCode} OK{CRLF}{CRLF}";

					byte[] data = Encoding.UTF8.GetBytes(response);

					stream.Write(data, 0, data.Length);
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

		public static void Stop()
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