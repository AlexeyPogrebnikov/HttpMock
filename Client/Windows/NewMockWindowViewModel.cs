using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HttpMock.Client.Commands;
using HttpMock.Core;

namespace HttpMock.Client.Windows
{
	public class NewMockWindowViewModel : INotifyPropertyChanged
	{
		private MockResponse _mock;
		public event PropertyChangedEventHandler PropertyChanged;
		public CreateMockCommand CreateMock { get; }

		public NewMockWindowViewModel()
		{
			var mockCache = ServiceLocator.Resolve<IMockCache>();
			CreateMock = new CreateMockCommand(mockCache);
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public MockResponse Mock
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
			CreateMock.CloseWindowAction = action;
		}
	}
}