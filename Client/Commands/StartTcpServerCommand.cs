using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using TcpMock.Core;

namespace TcpMock.Client.Commands
{
	public class StartTcpServerCommand : ICommand
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

			Task.Run(() => TcpServer.Start(address, port));
		}

		public event EventHandler CanExecuteChanged;
	}
}