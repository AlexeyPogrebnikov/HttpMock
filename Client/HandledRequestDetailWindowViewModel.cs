using System.ComponentModel;
using System.Runtime.CompilerServices;
using HttpMock.Core;

namespace HttpMock.Client
{
	public class HandledRequestDetailWindowViewModel : INotifyPropertyChanged
	{
		private HttpInteraction _httpInteraction;
		public event PropertyChangedEventHandler PropertyChanged;

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

				return $"Handled request at {HttpInteraction.Time}";
			}
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}