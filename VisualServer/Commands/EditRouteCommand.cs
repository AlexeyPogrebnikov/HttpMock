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
			var route = (Route)parameter;

			EditRouteWindow window = new();
			var viewModel = (EditRouteWindowViewModel) window.DataContext;
			viewModel.SetMainWindowViewModel(_mainWindowViewModel);
			viewModel.SetInitialRoute(route);
			window.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}