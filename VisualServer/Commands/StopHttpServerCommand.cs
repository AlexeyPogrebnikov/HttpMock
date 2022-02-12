using System;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.VisualServer.Commands
{
	public class StopHttpServerCommand : ICommand
	{
		private readonly IVisualHttpServer _httpServer;

		public StopHttpServerCommand(IVisualHttpServer httpServer)
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