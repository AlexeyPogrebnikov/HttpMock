using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HttpMock.Client.Commands;
using HttpMock.Core;

namespace HttpMock.Client.Windows
{
	public class NewRouteWindowViewModel : INotifyPropertyChanged
	{
		private Route _route;
		public event PropertyChangedEventHandler PropertyChanged;
		public CreateRouteCommand CreateRoute { get; }

		public NewRouteWindowViewModel()
		{
			var httpServer = ServiceLocator.Resolve<IHttpServer>();
			CreateRoute = new CreateRouteCommand(httpServer);
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public Route Route
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