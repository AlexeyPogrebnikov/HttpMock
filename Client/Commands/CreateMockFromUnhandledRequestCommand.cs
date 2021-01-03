using System;
using System.Windows.Input;
using HttpMock.Client.Windows;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class CreateMockFromUnhandledRequestCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var httpInteraction = (HttpInteraction) parameter;

			var window = new NewMockWindow();

			var dataContext = (NewMockWindowViewModel) window.DataContext;

			var mock = Mock.CreateNew();
			mock.Method = httpInteraction.Method;
			mock.Path = httpInteraction.Path;
			mock.StatusCode = "200";

			dataContext.NewMock = mock;

			window.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}