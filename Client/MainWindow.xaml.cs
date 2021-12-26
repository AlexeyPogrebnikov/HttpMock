using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.Client
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			var dataContext = (MainWindowViewModel)DataContext;
			dataContext.SetRefreshMocksListViewAction(RefreshMocksListView);
		}

		private void MockListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (sender is ListViewItem item)
			{
				if (item.Content is Route mock)
				{
					var dataContext = (MainWindowViewModel) DataContext;
					dataContext.SelectedMock = mock;
				}
			}
		}

		private void RefreshMocksListView()
		{
			MocksListView.Items.Refresh();
		}
	}
}