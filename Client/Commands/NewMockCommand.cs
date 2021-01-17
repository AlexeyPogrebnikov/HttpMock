﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HttpMock.Client.Windows;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class NewMockCommand : ICommand
	{
		private readonly IMockCache _mockCache;

		public NewMockCommand(IMockCache mockCache)
		{
			_mockCache = mockCache;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			if (_mockCache.GetAll().Count() >= Constants.MaxMocksCountInFreeVersion)
			{
				MessageBox.Show($"You can add more than {Constants.MaxMocksCountInFreeVersion} mocks in the Pro version!");
			}
			else
			{
				var window = new NewMockWindow();
				var newMockWindowViewModel = (NewMockWindowViewModel) window.DataContext;
				var mock = Mock.CreateNew();
				mock.StatusCode = "200";

				if (parameter is HttpInteraction httpInteraction)
				{
					mock.Method = httpInteraction.Method;
					mock.Path = httpInteraction.Path;
				}

				newMockWindowViewModel.Mock = mock;
				window.Show();
			}
		}

		public event EventHandler CanExecuteChanged;
	}
}