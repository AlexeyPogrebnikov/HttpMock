using System;
using System.Windows.Input;
using HttpMock.Client.Windows;

namespace HttpMock.Client.Commands
{
	public class AboutProgramCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var window = new AboutProgramWindow();
			window.ShowDialog();
		}

		public event EventHandler CanExecuteChanged;
	}
}