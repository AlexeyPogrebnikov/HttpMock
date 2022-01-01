using System;
using System.Windows;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class ClearRoutesCommand : ICommand
	{
		private readonly IHttpServer _httpServer;

		public ClearRoutesCommand(IHttpServer httpServer)
		{
			_httpServer = httpServer;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			if (MessageBox.Show("Are you sure you want to clear the route list?", "Clear routes", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				_httpServer.Routes.Clear();
			}
		}

		public event EventHandler CanExecuteChanged;
	}
}