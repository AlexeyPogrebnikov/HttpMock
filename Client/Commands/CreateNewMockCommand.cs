using System;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class CreateNewMockCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var window = new NewMockWindow();
			var newMockWindowViewModel = (NewMockWindowViewModel) window.DataContext;
			newMockWindowViewModel.NewMock = Mock.CreateNew();
			window.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}