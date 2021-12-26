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
		private Route _mock;
		public event PropertyChangedEventHandler PropertyChanged;
		public CreateRouteCommand CreateRoute { get; }

		public NewRouteWindowViewModel()
		{
			var mockCache = ServiceLocator.Resolve<IMockCache>();
			CreateRoute = new CreateRouteCommand(mockCache);
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public Route Mock
		{
			get => _mock;
			set
			{
				_mock = value;
				OnPropertyChanged(nameof(Mock));
			}
		}

		public IEnumerable<string> Methods => Constants.Methods;

		public IEnumerable<string> StatusCodes => Constants.StatusCodes;

		public void SetCloseWindowAction(Action action)
		{
			CreateRoute.CloseWindowAction = action;
		}
	}
}