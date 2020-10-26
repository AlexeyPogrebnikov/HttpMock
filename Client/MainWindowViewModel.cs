﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using TcpMock.Client.Commands;
using TcpMock.Core;

namespace TcpMock.Client
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		private Mock _selectedMock;
		private readonly ITcpServer _tcpServer;
		private readonly ITcpInteractionCache _tcpInteractionCache;

		public MainWindowViewModel()
		{
			_tcpServer = ServiceLocator.Resolve<ITcpServer>();
			var mockCache = ServiceLocator.Resolve<IMockCache>();
			if (mockCache != null)
			{
				IEnumerable<Mock> mocks = mockCache.GetAll();
				Mocks = new ObservableCollection<Mock>(mocks);
			}
			else
			{
				Mocks = new ObservableCollection<Mock>();
			}

			_tcpInteractionCache = ServiceLocator.Resolve<ITcpInteractionCache>();

			HandledRequests = new ObservableCollection<TcpInteraction>();
			UnhandledRequests = new ObservableCollection<TcpInteraction>();
			ConnectionSettings = ConnectionSettingsCache.ConnectionSettings;

			StartTcpServer = new StartTcpServerCommand(_tcpServer);
			StopTcpServer = new StopTcpServerCommand(_tcpServer);
			StartTcpServerVisibility = Visibility.Visible;
			StopTcpServerVisibility = Visibility.Hidden;

			var dispatcherTimer = new DispatcherTimer
			{
				Interval = new TimeSpan(0, 0, 0, 0, 100)
			};

			dispatcherTimer.Tick += DispatcherTimer_Tick;
			dispatcherTimer.Start();
		}

		private void DispatcherTimer_Tick(object sender, EventArgs e)
		{
			if (_tcpServer != null)
			{
				if (_tcpServer.IsStarted)
				{
					StartTcpServerVisibility = Visibility.Collapsed;
					StopTcpServerVisibility = Visibility.Visible;
				}
				else
				{
					StartTcpServerVisibility = Visibility.Visible;
					StopTcpServerVisibility = Visibility.Collapsed;
				}
			}

			OnPropertyChanged(nameof(StartTcpServerVisibility));
			OnPropertyChanged(nameof(StopTcpServerVisibility));

			if (_tcpInteractionCache != null)
			{
				IEnumerable<TcpInteraction> interactions = _tcpInteractionCache.GetAll();

				foreach (TcpInteraction interaction in interactions)
				{
					if (interaction.Handled)
					{
						if (HandledRequests.All(rp => rp.Uid != interaction.Uid))
							HandledRequests.Insert(0, interaction);
					}
					else
					{
						if (UnhandledRequests.All(rp => rp.Uid != interaction.Uid))
							UnhandledRequests.Insert(0, interaction);
					}
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public ObservableCollection<Mock> Mocks { get; }

		public ObservableCollection<TcpInteraction> HandledRequests { get; }
		public ObservableCollection<TcpInteraction> UnhandledRequests { get; }

		public ConnectionSettings ConnectionSettings { get; }

		public StartTcpServerCommand StartTcpServer { get; }
		public Visibility StartTcpServerVisibility { get; set; }
		public StopTcpServerCommand StopTcpServer { get; }
		public Visibility StopTcpServerVisibility { get; set; }

		public Mock SelectedMock
		{
			get => _selectedMock;
			set
			{
				_selectedMock = value;
				OnPropertyChanged(nameof(SelectedMock));
			}
		}

		public IEnumerable<string> Methods
		{
			get { return new[] { "GET", "POST" }; }
		}
	}
}