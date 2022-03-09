using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HttpMock.Core;
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
			var viewModel = (MainWindowViewModel)DataContext;
			viewModel.SetRefreshRoutesListViewAction(RefreshRoutesListView);
		}

		private void RouteListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (sender is not ListViewItem {Content: RouteUI route}) return;
			var dataContext = (MainWindowViewModel) DataContext;
			dataContext.EditRoute.Execute(route);
		}

		private void RefreshRoutesListView()
		{
			RoutesListView.Items.Refresh();
		}
	}
}