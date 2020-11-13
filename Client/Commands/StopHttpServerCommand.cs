using System;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class StopHttpServerCommand : ICommand
	{
		private readonly IHttpServer _httpServer;

		public StopHttpServerCommand(IHttpServer httpServer)
		{
			_httpServer = httpServer;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			_httpServer.Stop();
		}

		public event EventHandler CanExecuteChanged;
	}
}