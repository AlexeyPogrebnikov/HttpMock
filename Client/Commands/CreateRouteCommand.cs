using System;
using System.Windows;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.Client.Commands
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
			var mock = (Route) parameter;
			if (string.IsNullOrWhiteSpace(mock.Method) || string.IsNullOrWhiteSpace(mock.Path) || string.IsNullOrWhiteSpace(mock.Response.StatusCode))
			{
				MessageBox.Show("Please fill required (*) fields.");
				return;
			}

			if (_httpServer.Routes.Contains(mock))
			{
				MessageBox.Show("A route with same Method and Path already exists.");
				return;
			}

			_httpServer.Routes.Add(mock);
			CloseWindowAction();
		}

		public event EventHandler CanExecuteChanged;

		public Action CloseWindowAction { get; set; }
	}
}