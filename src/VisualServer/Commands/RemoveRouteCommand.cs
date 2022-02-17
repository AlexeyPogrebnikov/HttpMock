using System;
using System.Windows.Input;
using HttpMock.VisualServer.Model;

namespace HttpMock.VisualServer.Commands
{
	public class RemoveRouteCommand : ICommand
	{
		private readonly RouteUICollection _routes;

		public RemoveRouteCommand(RouteUICollection routes)
		{
			_routes = routes;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var route = (RouteUI) parameter;
			_routes.Remove(route);
		}

		public event EventHandler CanExecuteChanged;
	}
}