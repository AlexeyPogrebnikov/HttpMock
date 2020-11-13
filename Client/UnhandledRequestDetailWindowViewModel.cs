using System.ComponentModel;
using System.Runtime.CompilerServices;
using HttpMock.Client.Commands;
using HttpMock.Core;

namespace HttpMock.Client
{
	public class UnhandledRequestDetailWindowViewModel : INotifyPropertyChanged
	{
		private HttpInteraction _httpInteraction;
		public event PropertyChangedEventHandler PropertyChanged;
		public CreateMockFromUnhandledRequestCommand CreateMock { get; }

		public UnhandledRequestDetailWindowViewModel()
		{
			CreateMock = new CreateMockFromUnhandledRequestCommand();
		}

		public HttpInteraction HttpInteraction
		{
			get => _httpInteraction;
			set
			{
				_httpInteraction = value;
				OnPropertyChanged(nameof(HttpInteraction));
				OnPropertyChanged(nameof(Title));
			}
		}

		public string Title
		{
			get
			{
				if (HttpInteraction == null)
					return null;

				return $"Unhandled request at {HttpInteraction.Time}";
			}
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}