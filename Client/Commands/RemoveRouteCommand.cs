using System;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class RemoveRouteCommand : ICommand
	{
		private readonly IHttpServer _httpServer;

		public event EventHandler RouteCollectionChanged;

		public RemoveRouteCommand(IHttpServer httpServer)
		{
			_httpServer = httpServer;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var route = (Route) parameter;
			_httpServer.Routes.Remove(route);
			RouteCollectionChanged?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler CanExecuteChanged;
	}
}