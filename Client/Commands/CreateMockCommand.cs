using System;
using System.Windows;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class CreateMockCommand : ICommand
	{
		private readonly IMockCache _mockCache;

		public CreateMockCommand(IMockCache mockCache)
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
			if (string.IsNullOrWhiteSpace(mock.Method) || string.IsNullOrWhiteSpace(mock.StatusCode))
			{
				MessageBox.Show("Please fill required (*) fields.");
				return;
			}

			_mockCache.Add(mock);
			CloseWindowAction();
		}

		public event EventHandler CanExecuteChanged;

		public Action CloseWindowAction { get; set; }
	}
}