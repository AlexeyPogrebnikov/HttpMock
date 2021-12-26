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
			InitialMock.Path = changedMock.Path;
			InitialMock.Response.StatusCode = changedMock.Response.StatusCode;
			InitialMock.Response.Content = changedMock.Response.Content;
			CloseWindowAction();
			MainWindowViewModel.RefreshMocksListView();
		}

		public Action CloseWindowAction { get; set; }

		public event EventHandler CanExecuteChanged;

		public MockResponse InitialMock { get; set; }

		public IMainWindowViewModel MainWindowViewModel { get; set; }
	}
}