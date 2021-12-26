﻿using System;
using System.Windows.Input;
using HttpMock.Client.Windows;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class CreateMockFromUnhandledRequestCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var httpInteraction = (HttpInteraction) parameter;

			var window = new NewRouteWindow();

			var dataContext = (NewRouteWindowViewModel) window.DataContext;

			var mock = Route.CreateNew();
			mock.Method = httpInteraction.Method;
			mock.Path = httpInteraction.Path;
			mock.Response.StatusCode = "200";

			dataContext.Mock = mock;

			window.Show();
		}

		public event EventHandler CanExecuteChanged;
	}
}