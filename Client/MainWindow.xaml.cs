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
		}

		private void MockListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (sender is ListViewItem item)
			{
				if (item.Content is Mock mock)
				{
					var dataContext = (MainWindowViewModel) DataContext;
					dataContext.SelectedMock = mock;
				}
			}
		}

		private void HandledRequestsListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (sender is ListViewItem item)
			{
				if (item.Content is TcpInteraction interaction)
				{
					var window = new HandledRequestDetailWindow();
					var viewModel = (HandledRequestDetailWindowViewModel) window.DataContext;
					viewModel.TcpInteraction = interaction;
					window.ShowInTaskbar = false;
					window.Owner = this;
					window.Show();
				}
			}
		}

		private void UnhandledRequestsListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (sender is ListViewItem item)
			{
				if (item.Content is TcpInteraction interaction)
				{
					var window = new UnhandledRequestDetailWindow();
					var viewModel = (UnhandledRequestDetailWindowViewModel) window.DataContext;
					viewModel.TcpInteraction = interaction;
					window.ShowInTaskbar = false;
					window.Owner = this;
					window.Show();
				}
			}
		}
	}
}