using System;
using System.Windows.Input;
using HttpMock.Client.Windows;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class EditMockCommand : ICommand
	{
		private readonly IMainWindowViewModel _mainWindowViewModel;

		public EditMockCommand(IMainWindowViewModel mainWindowViewModel)
		{
			_mainWindowViewModel = mainWindowViewModel;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var window = new EditMockWindow();
			var newMockWindowViewModel = (EditMockWindowViewModel) window.DataContext;
			newMockWindowViewModel.SetMainWindowViewModel(_mainWindowViewModel);
			var mock = (MockResponse) parameter;
			newMockWindowViewModel.SetInitialMock(mock);
			newMockWindowViewModel.Mock = mock.Clone();
			window.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}