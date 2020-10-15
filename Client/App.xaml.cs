using System.Windows;
using TcpMock.Core;

namespace TcpMock.Client
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			MockCache.Add(new Mock
			{
				Method = "GET",
				Path = "/"
			});

			MockCache.Add(new Mock
			{
				Method = "POST",
				Path = "/"
			});
		}
	}
}