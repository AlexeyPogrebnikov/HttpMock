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
			var newMockWindowViewModel = (NewRouteWindowViewModel) window.DataContext;
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