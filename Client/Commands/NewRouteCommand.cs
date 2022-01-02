using System;
using System.Windows.Input;
using HttpMock.Client.Windows;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class NewRouteCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			NewRouteWindow window = new();
			var newRouteWindowViewModel = (NewRouteWindowViewModel) window.DataContext;
			var route = Route.CreateNew();
			route.Response.StatusCode = "200";

			if (parameter is HttpInteraction httpInteraction)
			{
				route.Method = httpInteraction.Method;
				route.Path = httpInteraction.Path;
			}

			newRouteWindowViewModel.Route = route;
			window.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}