﻿using System;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class CreateNewMockCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			NewMockWindow window = new NewMockWindow();
			var newMockWindowViewModel = (NewMockWindowViewModel)window.DataContext;
			newMockWindowViewModel.NewMock = new Mock();
			window.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}