using System.ComponentModel;
using HttpMock.Core;

namespace HttpMock.VisualServer.Windows
{
	public class AboutProgramWindowViewModel : INotifyPropertyChanged
	{
		public string Version => VersionHelper.GetCurrentAppVersion();

		public string Author => "Alexey Pogrebnikov";

		public event PropertyChangedEventHandler PropertyChanged;
	}
}