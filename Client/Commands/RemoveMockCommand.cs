using System;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.Client.Commands
{
	public class RemoveMockCommand : ICommand
	{
		private readonly IHttpServer _httpServer;

		public event EventHandler MockCollectionChanged;

		public RemoveMockCommand(IHttpServer httpServer)
		{
			_httpServer = httpServer;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var mock = (Route) parameter;
			_httpServer.Routes.Remove(mock);
			MockCollectionChanged?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler CanExecuteChanged;
	}
}