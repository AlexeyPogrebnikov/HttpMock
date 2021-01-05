using System;
using System.Windows.Input;

namespace HttpMock.Client.Commands
{
	public class ClearHandledRequestsCommand : ICommand
	{
		private readonly IMainWindowViewModel _mainWindowViewModel;

		public ClearHandledRequestsCommand(IMainWindowViewModel mainWindowViewModel)
		{
			_mainWindowViewModel = mainWindowViewModel;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_mainWindowViewModel.HandledRequests.Clear();
		}

		public event EventHandler CanExecuteChanged;
	}
}