using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HttpMock.Client.Commands;
using HttpMock.Core;

namespace HttpMock.Client.Windows
{
	public class EditMockWindowViewModel : INotifyPropertyChanged
	{
		private Route _mock;
		public event PropertyChangedEventHandler PropertyChanged;
		public SaveRouteCommand SaveMock { get; }

		public EditMockWindowViewModel()
		{
			SaveMock = new SaveRouteCommand();
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void SetInitialMock(Route mock)
		{
			SaveMock.InitialRoute = mock;
		}

		public Route Route
		{
			get => _mock;
			set
			{
				_mock = value;
				OnPropertyChanged(nameof(Route));
			}
		}

		public IEnumerable<string> Methods => Constants.Methods;

		public IEnumerable<string> StatusCodes => Constants.StatusCodes;

		public void SetCloseWindowAction(Action action)
		{
			SaveMock.CloseWindowAction = action;
		}

		public void SetMainWindowViewModel(IMainWindowViewModel mainWindowViewModel)
		{
			SaveMock.MainWindowViewModel = mainWindowViewModel;
		}
	}
}