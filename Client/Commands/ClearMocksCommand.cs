using System;
using System.Windows;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class ClearMocksCommand : ICommand
	{
		private readonly IMockCache _mockCache;

		public ClearMocksCommand(IMockCache mockCache)
		{
			_mockCache = mockCache;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			if (MessageBox.Show("Are you sure you want to clear the mock list?", "Clear mock list", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				_mockCache.Clear();
			}
		}

		public event EventHandler CanExecuteChanged;
	}
}