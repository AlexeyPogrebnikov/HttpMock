using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
					RequestCache.Add(new Request
					{
						Time = DateTime.Now.TimeOfDay,
						Url = Encoding.UTF8.GetString(buffer)
					});

					var response = $"HTTP/1.1 200 OK{CRLF}{CRLF}";
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
				if (_server != null)
					_server.Stop();
			}
		}

		public static void Stop()
		{
			_server.Stop();
		}
	}
}