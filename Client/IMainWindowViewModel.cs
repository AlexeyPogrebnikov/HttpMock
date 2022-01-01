using System.Collections.ObjectModel;
using HttpMock.Core;

namespace HttpMock.Client
{
	public interface IMainWindowViewModel
	{
		ObservableCollection<HttpInteraction> HandledRequests { get; }
		ObservableCollection<HttpInteraction> UnhandledRequests { get; }
		void RefreshRouteListView();
	}
}