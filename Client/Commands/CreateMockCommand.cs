using System;
using System.Windows.Input;
using TcpMock.Core;

namespace TcpMock.Client.Commands
{
	public class CreateMockCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var mock = (Mock) parameter;
			MockCache.Add(mock);
			CloseWindowAction();
		}

		public event EventHandler CanExecuteChanged;

		public Action CloseWindowAction { get; set; }
	}
}