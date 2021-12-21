using System;
using System.Windows.Input;
using HttpMock.Client.Windows;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class NewMockCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var window = new NewMockWindow();
			var newMockWindowViewModel = (NewMockWindowViewModel) window.DataContext;
			var mock = MockResponse.CreateNew();
			mock.StatusCode = "200";

			if (parameter is HttpInteraction httpInteraction)
			{
				mock.Method = httpInteraction.Method;
				mock.Path = httpInteraction.Path;
			}

			newMockWindowViewModel.Mock = mock;
			window.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}