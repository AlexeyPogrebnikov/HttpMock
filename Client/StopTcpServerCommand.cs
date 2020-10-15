using System;
using System.Windows.Input;
using TcpMock.Core;

namespace TcpMock.Client
{
	public class StopTcpServerCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			TcpServer.Stop();
		}

		public event EventHandler CanExecuteChanged;
	}
}