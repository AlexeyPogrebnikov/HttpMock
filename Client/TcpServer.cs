using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TcpMock.Core;

namespace TcpMock.Client
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
					TcpClient client = _server.AcceptTcpClient();
					NetworkStream stream = client.GetStream();
					var buffer = new byte[1024];
					stream.Read(buffer, 0, 1024);
					string content = Encoding.UTF8.GetString(buffer);
					Request request = requestParser.Parse(content);
					request.Time = DateTime.Now.TimeOfDay;
					Mock mock = MockCache.GetAll().FirstOrDefault(m => m.Path == request.Path);
					request.Handled = mock != null;
					RequestCache.Add(request);

					var responseCode = "200";
					if (mock == null)
						responseCode = "404";

					var response = $"HTTP/1.1 {responseCode} OK{CRLF}{CRLF}";

					byte[] data = Encoding.UTF8.GetBytes(response);

					stream.Write(data, 0, data.Length);
					stream.Close();
					client.Close();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				_server = null;
			}
			finally
			{
				_server?.Stop();
			}
		}

		public static void Stop()
		{
			_server?.Stop();
		}
	}
}