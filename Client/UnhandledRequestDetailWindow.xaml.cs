using System.Windows;

namespace HttpMock.Client
{
	/// <summary>
	/// Interaction logic for UnhandledRequestDetailWindow.xaml
	/// </summary>
	public partial class UnhandledRequestDetailWindow : Window
	{
		public UnhandledRequestDetailWindow()
		{
			InitializeComponent();
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}