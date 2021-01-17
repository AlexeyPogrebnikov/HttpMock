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
		private Mock _mock;
		public event PropertyChangedEventHandler PropertyChanged;
		public SaveMockCommand SaveMock { get; }

		public EditMockWindowViewModel()
		{
			SaveMock = new SaveMockCommand();
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void SetInitialMock(Mock mock)
		{
			SaveMock.InitialMock = mock;
		}

		public Mock Mock
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
			SaveMock.CloseWindowAction = action;
		}
	}
}