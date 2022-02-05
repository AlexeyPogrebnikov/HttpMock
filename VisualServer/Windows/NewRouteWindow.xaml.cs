﻿using System.Windows;

namespace HttpMock.VisualServer.Windows
{
	public partial class NewRouteWindow : Window
	{
		public NewRouteWindow()
		{
			InitializeComponent();
			var dataContext = (NewRouteWindowViewModel) DataContext;
			dataContext.SetCloseWindowAction(Close);
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}