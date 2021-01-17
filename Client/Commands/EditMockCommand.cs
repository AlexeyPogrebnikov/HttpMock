using System;
using System.Windows.Input;
using HttpMock.Client.Windows;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class EditMockCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var window = new EditMockWindow();
			var newMockWindowViewModel = (EditMockWindowViewModel) window.DataContext;
			var mock = (Mock) parameter;
			newMockWindowViewModel.SetInitialMock(mock);
			newMockWindowViewModel.Mock = mock.Clone();
			window.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}