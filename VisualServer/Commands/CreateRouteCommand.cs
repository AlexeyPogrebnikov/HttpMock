using System;
using System.Linq;
using System.Windows.Input;
using HttpMock.VisualServer.Model;

namespace HttpMock.VisualServer.Commands
{
	public class CreateRouteCommand : ICommand
	{
		private readonly RouteUICollection _routes;
		private readonly IMessageViewer _messageViewer;

		public CreateRouteCommand(RouteUICollection routes, IMessageViewer messageViewer)
		{
			_routes = routes;
			_messageViewer = messageViewer;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var route = (RouteUI)parameter;
			if (string.IsNullOrWhiteSpace(route.Method) || string.IsNullOrWhiteSpace(route.Path))
			{
				_messageViewer.View(string.Empty, "Please fill required (*) fields.");
				return;
			}

			if (_routes.Any(rt => rt.Method == route.Method && rt.Path == route.Path))
			{
				_messageViewer.View(string.Empty, "A route with same Method and Path already exists.");
				return;
			}

			_routes.Add(route);
			CloseWindowAction?.Invoke();
		}

		public event EventHandler CanExecuteChanged;

		public Action CloseWindowAction { get; set; }
	}
}