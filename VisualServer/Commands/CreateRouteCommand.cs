using System;
using System.Windows;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.VisualServer.Commands
{
	public class CreateRouteCommand : ICommand
	{
		private readonly IHttpServer _httpServer;

		public CreateRouteCommand(IHttpServer httpServer)
		{
			_httpServer = httpServer;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var route = (Route) parameter;
			if (string.IsNullOrWhiteSpace(route.Method) || string.IsNullOrWhiteSpace(route.Path))
			{
				MessageBox.Show("Please fill required (*) fields.");
				return;
			}

			if (_httpServer.Routes.Contains(route))
			{
				MessageBox.Show("A route with same Method and Path already exists.");
				return;
			}

			_httpServer.Routes.Add(route);
			CloseWindowAction();
		}

		public event EventHandler CanExecuteChanged;

		public Action CloseWindowAction { get; set; }
	}
}