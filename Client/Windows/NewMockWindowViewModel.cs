﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HttpMock.Client.Commands;
using HttpMock.Core;

namespace HttpMock.Client.Windows
{
	public class NewMockWindowViewModel : INotifyPropertyChanged
	{
		private Mock _newMock;
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

		public Mock NewMock
		{
			get => _newMock;
			set
			{
				_newMock = value;
				OnPropertyChanged(nameof(NewMock));
			}
		}

		public IEnumerable<string> Methods
		{
			get { return new[] { "GET", "POST", "PUT", "DELETE" }; }
		}

		public IEnumerable<string> StatusCodes
		{
			get { return new[] { "200", "400", "401", "403", "404", "500" }; }
		}

		public void SetCloseWindowAction(Action action)
		{
			CreateMock.CloseWindowAction = action;
		}
	}
}