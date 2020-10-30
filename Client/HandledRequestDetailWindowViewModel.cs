﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using TcpMock.Core;

namespace TcpMock.Client
{
	public class HandledRequestDetailWindowViewModel : INotifyPropertyChanged
	{
		private TcpInteraction _tcpInteraction;
		public event PropertyChangedEventHandler PropertyChanged;

		public TcpInteraction TcpInteraction
		{
			get => _tcpInteraction;
			set
			{
				_tcpInteraction = value;
				OnPropertyChanged(nameof(TcpInteraction));
				OnPropertyChanged(nameof(Title));
			}
		}

		public string Title
		{
			get
			{
				if (TcpInteraction == null)
					return null;

				return $"Handled request at {TcpInteraction.Time}";
			}
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}