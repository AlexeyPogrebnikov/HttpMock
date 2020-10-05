using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using TcpMock.Client.Annotations;

namespace TcpMock.Client
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		public MainWindowViewModel()
		{
			//TODO test data
			var mockListViewItems = new[]
			{
				new MockListViewItem
				{
					Caption = "GET /"
				},
				new MockListViewItem
				{
					Caption = "POST /Book/"
				}
			};

			MockListViewItems = new ObservableCollection<MockListViewItem>(mockListViewItems);
			RequestListViewItems = new ObservableCollection<RequestListViewItem>();
			ConnectionSettings = new ConnectionSettings
			{
				Host = "127.0.0.1",
				Port = 5000
			};
			Connect = new ConnectCommand();
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
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public ObservableCollection<MockListViewItem> MockListViewItems { get; }

		public ObservableCollection<RequestListViewItem> RequestListViewItems { get; }

		public ConnectionSettings ConnectionSettings { get; }

		public ConnectCommand Connect { get; }

		public string RequestsHeader => $"Requests ({RequestListViewItems.Count})";
	}
}