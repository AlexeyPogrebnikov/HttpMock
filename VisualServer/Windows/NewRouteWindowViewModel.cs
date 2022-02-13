using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HttpMock.VisualServer.Commands;
using HttpMock.VisualServer.Model;

namespace HttpMock.VisualServer.Windows
{
	public class NewRouteWindowViewModel : INotifyPropertyChanged
	{
		private RouteUI _route;
		public event PropertyChangedEventHandler PropertyChanged;
		public CreateRouteCommand CreateRoute { get; }

		public NewRouteWindowViewModel()
		{
			RouteUICollection routes = ServiceLocator.Resolve<RouteUICollection>();
			IMessageViewer messageViewer = ServiceLocator.Resolve<IMessageViewer>();
			CreateRoute = new CreateRouteCommand(routes, messageViewer);
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public RouteUI Route
		{
			get => _route;
			set
			{
				_route = value;
				OnPropertyChanged(nameof(Route));
			}
		}

		public IEnumerable<string> Methods => Constants.Methods;

		public IEnumerable<int> StatusCodes => Constants.StatusCodes;

		public void SetCloseWindowAction(Action action)
		{
			CreateRoute.CloseWindowAction = action;
		}
	}
}