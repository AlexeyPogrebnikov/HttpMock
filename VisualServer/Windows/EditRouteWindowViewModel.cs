using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HttpMock.VisualServer.Commands;
using HttpMock.Core;

namespace HttpMock.VisualServer.Windows
{
	public class EditRouteWindowViewModel : INotifyPropertyChanged
	{
		private Route _route;
		public event PropertyChangedEventHandler PropertyChanged;
		public SaveRouteCommand SaveRoute { get; }

		public EditRouteWindowViewModel()
		{
			SaveRoute = new SaveRouteCommand();
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void SetInitialRoute(Route route)
		{
			SaveRoute.InitialRoute = route;
			Route = route.Clone();
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

		public static IEnumerable<string> Methods => Constants.Methods;

		public static IEnumerable<int> StatusCodes => Constants.StatusCodes;

		public void SetCloseWindowAction(Action action)
		{
			SaveRoute.CloseWindowAction = action;
		}

		public void SetMainWindowViewModel(IMainWindowViewModel mainWindowViewModel)
		{
			SaveRoute.MainWindowViewModel = mainWindowViewModel;
		}
	}
}