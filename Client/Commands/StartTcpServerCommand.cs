using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class StartTcpServerCommand : ICommand
	{
		private readonly ITcpServer _tcpServer;

		public StartTcpServerCommand(ITcpServer tcpServer)
		{
			_tcpServer = tcpServer;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var connectionSettings = (ConnectionSettings) parameter;

			IPAddress address = IPAddress.Parse(connectionSettings.Host);
			int port = connectionSettings.Port;

			Task.Run(() => _tcpServer.Start(address, port));
		}

		public event EventHandler CanExecuteChanged;
	}
}