using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using TcpMock.Client.Annotations;

namespace TcpMock.Client
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		private Mock _selectedMock;

		public MainWindowViewModel()
		{
			//TODO test data
			var mocks = new[]
			{
				new Mock
				{
					Method = "GET",
					Path = "/"
				},
				new Mock
				{
					Method = "POST",
					Path = "/Book/"
				}
			};

			Mocks = new ObservableCollection<Mock>(mocks);
			RequestListViewItems = new ObservableCollection<RequestListViewItem>();
			ConnectionSettings = new ConnectionSettings
			{
				Host = "127.0.0.1",
				Port = 5000
			};

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
			RequestListViewItems.Clear();
			IEnumerable<Request> requests = RequestCache.GetAll().Reverse();
			foreach (Request request in requests)
			{
				RequestListViewItems.Add(new RequestListViewItem
				{
					Time = request.Time,
					Url = request.Url
				});
			}

			OnPropertyChanged(nameof(RequestsHeader));

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
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public ObservableCollection<Mock> Mocks { get; }

		public ObservableCollection<RequestListViewItem> RequestListViewItems { get; }

		public ConnectionSettings ConnectionSettings { get; }

		public StartTcpServerCommand StartTcpServer { get; }
		public Visibility StartTcpServerVisibility { get; set; }
		public StopTcpServerCommand StopTcpServer { get; }
		public Visibility StopTcpServerVisibility { get; set; }

		public string RequestsHeader => $"Requests ({RequestListViewItems.Count})";

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