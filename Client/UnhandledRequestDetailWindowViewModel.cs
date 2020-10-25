using System.ComponentModel;
using System.Runtime.CompilerServices;
using TcpMock.Client.Commands;
using TcpMock.Core;

namespace TcpMock.Client
{
	public class UnhandledRequestDetailWindowViewModel : INotifyPropertyChanged
	{
		private TcpInteraction _tcpInteraction;
		public event PropertyChangedEventHandler PropertyChanged;
		public CreateMockFromUnhandledRequestCommand CreateMock { get; }

		public UnhandledRequestDetailWindowViewModel()
		{
			CreateMock = new CreateMockFromUnhandledRequestCommand();
		}

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

				return $"Unhandled request at {TcpInteraction.Time}";
			}
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}