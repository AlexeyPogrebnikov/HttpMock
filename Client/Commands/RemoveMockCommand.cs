using System;
using System.Windows.Input;
using TcpMock.Core;

namespace TcpMock.Client.Commands
{
	public class RemoveMockCommand : ICommand
	{
		private readonly IMockCache _mockCache;

		public event EventHandler MockCollectionChanged;

		public RemoveMockCommand(IMockCache mockCache)
		{
			_mockCache = mockCache;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var mock = (Mock) parameter;
			_mockCache.Remove(mock);
			MockCollectionChanged?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler CanExecuteChanged;
	}
}