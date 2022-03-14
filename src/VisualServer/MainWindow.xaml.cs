using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HttpMock.VisualServer.Model;

namespace HttpMock.VisualServer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			ViewModel.SetRefreshRoutesListViewAction(RefreshRoutesListView);
		}

		private MainWindowViewModel ViewModel => (MainWindowViewModel) DataContext;

		private void RouteListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (sender is not ListViewItem {Content: RouteUI route}) return;
			ViewModel.EditRoute.Execute(route);
		}

		private void RouteListView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete && sender is ListViewItem {Content: RouteUI route})
				ViewModel.RemoveRoute.Execute(route);
		}

		private void RefreshRoutesListView()
		{
			RoutesListView.Items.Refresh();
		}
	}
}