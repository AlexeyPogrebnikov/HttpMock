using System.Windows;

namespace HttpMock.Client.Windows
{
	/// <summary>
	/// Interaction logic for NewMockWindow.xaml
	/// </summary>
	public partial class NewMockWindow : Window
	{
		public NewMockWindow()
		{
			InitializeComponent();
			var dataContext = (NewMockWindowViewModel) DataContext;
			dataContext.SetCloseWindowAction(() => Close());
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}