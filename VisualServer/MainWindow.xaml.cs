using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HttpMock.Core;

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

		private void RouteListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (sender is ListViewItem item)
			{
				if (item.Content is Route route)
				{
					var dataContext = (MainWindowViewModel) DataContext;
					dataContext.SelectedRoute = route;
				}
			}
		}

		private void RefreshRoutesListView()
		{
			RoutesListView.Items.Refresh();
		}
	}
}