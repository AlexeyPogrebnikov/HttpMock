using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TcpMock.Core;

namespace TcpMock.Client
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

		private void RequestsListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (sender is ListViewItem item)
			{
				if (item.Content is TcpInteraction interaction)
				{
					var window = new RequestDetailWindow();
					var viewModel = (TcpInteractionDetailWindowViewModel) window.DataContext;
					viewModel.TcpInteraction = interaction;
					window.ShowInTaskbar = false;
					window.Owner = this;
					window.Show();
				}
			}
		}
	}
}