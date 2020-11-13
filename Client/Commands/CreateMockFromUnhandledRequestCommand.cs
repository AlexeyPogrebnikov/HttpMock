using System;
using System.Windows.Input;
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
			dataContext.NewMock = new Mock
			{
				Uid = Guid.NewGuid(),
				Method = httpInteraction.Method,
				Path = httpInteraction.Path,
				StatusCode = "200"
			};

			window.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}