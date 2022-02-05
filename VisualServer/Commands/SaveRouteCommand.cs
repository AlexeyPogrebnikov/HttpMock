﻿using System;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.VisualServer.Commands
{
	public class SaveRouteCommand : ICommand
	{
		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var changedRoute = (Route) parameter;
			InitialRoute.Method = changedRoute.Method;
			InitialRoute.Path = changedRoute.Path;
			InitialRoute.Response = changedRoute.Response;
			CloseWindowAction();
			MainWindowViewModel.RefreshRouteListView();
		}

		public Action CloseWindowAction { get; set; }

		public event EventHandler CanExecuteChanged;

		public Route InitialRoute { get; set; }

		public IMainWindowViewModel MainWindowViewModel { get; set; }
	}
}