using System;
using System.Windows.Input;

namespace HttpMock.Client.Commands
{
	public class ClearUnhandledRequestsCommand : ICommand
	{
		private readonly IMainWindowViewModel _mainWindowViewModel;

		public ClearUnhandledRequestsCommand(IMainWindowViewModel mainWindowViewModel)
		{
			_mainWindowViewModel = mainWindowViewModel;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_mainWindowViewModel.UnhandledRequests.Clear();
		}

		public event EventHandler CanExecuteChanged;
	}
}