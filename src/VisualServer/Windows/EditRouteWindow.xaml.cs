using System.Windows;

namespace HttpMock.VisualServer.Windows
{
	/// <summary>
	/// Interaction logic for EditMockWindow.xaml
	/// </summary>
	public partial class EditRouteWindow : Window
	{
		public EditRouteWindow()
		{
			InitializeComponent();
			var dataContext = (EditRouteWindowViewModel) DataContext;
			dataContext.SetCloseWindowAction(Close);
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}