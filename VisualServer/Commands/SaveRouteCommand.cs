using System;
using System.Linq;
using System.Windows.Input;
using HttpMock.VisualServer.Model;

namespace HttpMock.VisualServer.Commands
{
	public class SaveRouteCommand : ICommand
	{
		private readonly IMessageViewer _messageViewer;
		private readonly RouteUICollection _routes;

		public SaveRouteCommand(RouteUICollection routes, IMessageViewer messageViewer)
		{
			_routes = routes;
			_messageViewer = messageViewer;
		}

		public Action CloseWindowAction { get; set; }

		public RouteUI InitialRoute { get; set; }

		public IMainWindowViewModel MainWindowViewModel { get; set; }

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var changedRoute = (RouteUI) parameter;
			if (string.IsNullOrWhiteSpace(changedRoute.Method) || string.IsNullOrWhiteSpace(changedRoute.Path))
			{
				_messageViewer.View(string.Empty, "Please fill required (*) fields.");
				return;
			}

			if (RouteAlreadyExists(changedRoute))
			{
				_messageViewer.View(string.Empty, "A route with same Method and Path already exists.");
				return;
			}

			_routes.Update(InitialRoute, changedRoute);
			CloseWindowAction?.Invoke();
			MainWindowViewModel.RefreshRouteListView();
		}

		public event EventHandler CanExecuteChanged;

		private bool RouteAlreadyExists(RouteUI changedRoute)
		{
			return _routes
				.Except(new[] {InitialRoute})
				.Any(rt => rt.Method == changedRoute.Method && rt.Path == changedRoute.Path);
		}
	}
}