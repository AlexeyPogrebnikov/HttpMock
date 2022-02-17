using System.Collections.ObjectModel;
using HttpMock.Core;

namespace HttpMock.VisualServer
{
	public interface IMainWindowViewModel
	{
		ObservableCollection<Interaction> HandledRequests { get; }
		ObservableCollection<Interaction> UnhandledRequests { get; }
		void RefreshRouteListView();
	}
}