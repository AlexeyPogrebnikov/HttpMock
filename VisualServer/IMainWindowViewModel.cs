using System.Collections.ObjectModel;
using HttpMock.Core;

namespace HttpMock.VisualServer
{
	public interface IMainWindowViewModel
	{
		ObservableCollection<HttpInteraction> HandledRequests { get; }
		ObservableCollection<HttpInteraction> UnhandledRequests { get; }
		void RefreshRouteListView();
	}
}