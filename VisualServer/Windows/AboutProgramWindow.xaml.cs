﻿using System.Windows;

namespace HttpMock.VisualServer.Windows
{
	/// <summary>
	/// Interaction logic for AboutProgramWindow.xaml
	/// </summary>
	public partial class AboutProgramWindow : Window
	{
		public AboutProgramWindow()
		{
			InitializeComponent();
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}