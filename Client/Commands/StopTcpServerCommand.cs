﻿using System;
using System.Windows.Input;
using TcpMock.Core;

namespace TcpMock.Client.Commands
{
	public class StopTcpServerCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var tcpServer = ServiceLocator.Resolve<ITcpServer>();
			tcpServer.Stop();
		}

		public event EventHandler CanExecuteChanged;
	}
}