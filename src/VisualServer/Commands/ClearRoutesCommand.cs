using System;
using System.Windows;
using System.Windows.Input;
using HttpMock.VisualServer.Model;

namespace HttpMock.VisualServer.Commands
{
	public class ClearRoutesCommand : ICommand
	{
		private readonly RouteUICollection _routes;

		public ClearRoutesCommand(RouteUICollection routes)
		{
			_routes = routes;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			if (MessageBox.Show("Are you sure you want to clear the route list?", "Clear routes", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				_routes.Clear();
		}

		public event EventHandler CanExecuteChanged;
	}
}