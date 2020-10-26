using System;
using System.Windows.Input;
using TcpMock.Core;

namespace TcpMock.Client.Commands
{
	public class StopTcpServerCommand : ICommand
	{
		private readonly ITcpServer _tcpServer;

		public StopTcpServerCommand(ITcpServer tcpServer)
		{
			_tcpServer = tcpServer;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_tcpServer.Stop();
		}

		public event EventHandler CanExecuteChanged;
	}
}