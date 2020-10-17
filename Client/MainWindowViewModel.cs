using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using TcpMock.Core;

namespace TcpMock.Client
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		private Mock _selectedMock;

		public MainWindowViewModel()
		{
			IEnumerable<Mock> mocks = MockCache.GetAll();

			Mocks = new ObservableCollection<Mock>(mocks);
			HandledRequests = new ObservableCollection<TcpInteraction>();
			UnhandledRequests = new ObservableCollection<TcpInteraction>();
			ConnectionSettings = ConnectionSettingsCache.ConnectionSettings;

			StartTcpServer = new StartTcpServerCommand();
			StopTcpServer = new StopTcpServerCommand();
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
			if (TcpServer.IsStarted)
			{
				StartTcpServerVisibility = Visibility.Collapsed;
				StopTcpServerVisibility = Visibility.Visible;
			}
			else
			{
				StartTcpServerVisibility = Visibility.Visible;
				StopTcpServerVisibility = Visibility.Collapsed;
			}

			OnPropertyChanged(nameof(StartTcpServerVisibility));
			OnPropertyChanged(nameof(StopTcpServerVisibility));

			IEnumerable<TcpInteraction> interactions = TcpInteractionCache.GetAll();

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