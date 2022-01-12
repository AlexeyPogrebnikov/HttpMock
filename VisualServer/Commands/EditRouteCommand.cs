using System;
using System.Windows.Input;
using HttpMock.VisualServer.Windows;
using HttpMock.Core;

namespace HttpMock.VisualServer.Commands
{
	public class EditRouteCommand : ICommand
	{
		private readonly IMainWindowViewModel _mainWindowViewModel;

		public EditRouteCommand(IMainWindowViewModel mainWindowViewModel)
		{
			_mainWindowViewModel = mainWindowViewModel;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var window = new EditRouteWindow();
			var newRouteWindowViewModel = (EditRouteWindowViewModel) window.DataContext;
			newRouteWindowViewModel.SetMainWindowViewModel(_mainWindowViewModel);
			var route = (Route) parameter;
			newRouteWindowViewModel.SetInitialRoute(route);
			newRouteWindowViewModel.Route = route.Clone();
			window.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}