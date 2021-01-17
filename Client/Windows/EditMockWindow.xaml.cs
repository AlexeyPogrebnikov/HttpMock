using System.Windows;

namespace HttpMock.Client.Windows
{
	/// <summary>
	/// Interaction logic for EditMockWindow.xaml
	/// </summary>
	public partial class EditMockWindow : Window
	{
		public EditMockWindow()
		{
			InitializeComponent();
			var dataContext = (EditMockWindowViewModel) DataContext;
			dataContext.SetCloseWindowAction(Close);
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}