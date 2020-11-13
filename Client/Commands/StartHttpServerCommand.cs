using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class StartHttpServerCommand : ICommand
	{
		private readonly IHttpServer _httpServer;

		public StartHttpServerCommand(IHttpServer httpServer)
		{
			_httpServer = httpServer;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var connectionSettings = (ConnectionSettings) parameter;

			IPAddress address = IPAddress.Parse(connectionSettings.Host);
			int port = connectionSettings.Port;

			Task.Run(() => _httpServer.Start(address, port));
		}

		public event EventHandler CanExecuteChanged;
	}
}