using System;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class SaveMockCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var changedMock = (MockResponse) parameter;
			InitialMock.Method = changedMock.Method;
			InitialMock.StatusCode = changedMock.StatusCode;
			InitialMock.Path = changedMock.Path;
			InitialMock.Content = changedMock.Content;
			CloseWindowAction();
			MainWindowViewModel.RefreshMocksListView();
		}

		public Action CloseWindowAction { get; set; }

		public event EventHandler CanExecuteChanged;

		public MockResponse InitialMock { get; set; }

		public IMainWindowViewModel MainWindowViewModel { get; set; }
	}
}