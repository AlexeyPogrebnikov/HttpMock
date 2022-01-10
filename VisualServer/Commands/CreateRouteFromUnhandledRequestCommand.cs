using System;
using System.Windows.Input;
using HttpMock.VisualServer.Windows;
using HttpMock.Core;

namespace HttpMock.VisualServer.Commands
{
	public class CreateRouteFromUnhandledRequestCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var httpInteraction = (HttpInteraction) parameter;

			NewRouteWindow window = new();

			var dataContext = (NewRouteWindowViewModel) window.DataContext;

			Route route = new()
			{
				Response = new Response()
				{
					StatusCode = 200
				}
			};

			route.Method = httpInteraction.Method;
			route.Path = httpInteraction.Path;

			dataContext.Route = route;

			window.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}