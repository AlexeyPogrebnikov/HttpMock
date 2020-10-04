using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
			ConnectionSettings = new ConnectionSettings
			{
				Host = "127.0.0.1",
				Port = 5000
			};
			Connect = new ConnectCommand();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public ObservableCollection<MockListViewItem> MockListViewItems { get; }

		public ConnectionSettings ConnectionSettings { get; }

		public ConnectCommand Connect { get; }
	}
}