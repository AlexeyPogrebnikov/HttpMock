using System;
using System.Windows.Input;
using TcpMock.Core;

namespace TcpMock.Client.Commands
{
	public class CreateMockFromUnhandledRequestCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var tcpInteraction = (TcpInteraction) parameter;

			var window = new NewMockWindow();

			var dataContext = (NewMockWindowViewModel) window.DataContext;
			dataContext.NewMock = new Mock
			{
				Method = tcpInteraction.Method,
				Path = tcpInteraction.Path,
				StatusCode = "200"
			};

			window.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}