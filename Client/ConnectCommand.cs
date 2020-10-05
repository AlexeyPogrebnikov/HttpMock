using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TcpMock.Client
{
	public class ConnectCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var connectionSettings = (ConnectionSettings) parameter;

			IPAddress address = IPAddress.Parse(connectionSettings.Host);
			int port = connectionSettings.Port;

			Task.Run(() => { DoListen(address, port); });
		}

		public event EventHandler CanExecuteChanged;

		private static void DoListen(IPAddress localAddr, int port)
		{
			TcpListener server = null;
			try
			{
				server = new TcpListener(localAddr, port);

				server.Start();

				while (true)
				{
					TcpClient client = server.AcceptTcpClient();
					NetworkStream stream = client.GetStream();
					var buffer = new byte[1024];
					stream.Read(buffer, 0, 1024);
					RequestCache.Add(new Request
					{
						Time = DateTime.Now.TimeOfDay,
						Url = Encoding.ASCII.GetString(buffer)
					});

					var response = "Привет мир";
					byte[] data = Encoding.UTF8.GetBytes(response);

					stream.Write(data, 0, data.Length);
					stream.Close();
					client.Close();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				if (server != null)
					server.Stop();
			}
		}
	}
}