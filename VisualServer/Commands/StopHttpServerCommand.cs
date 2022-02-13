using System;
using System.Windows.Input;
using Serilog;

namespace HttpMock.VisualServer.Commands
{
	public class StopHttpServerCommand : ICommand
	{
		private readonly IVisualHttpServer _httpServer;
		private readonly IMessageViewer _messageViewer;

		public StopHttpServerCommand(IVisualHttpServer httpServer, IMessageViewer messageViewer)
		{
			_httpServer = httpServer;
			_messageViewer = messageViewer;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public async void Execute(object parameter)
		{
			try
			{
				await _httpServer.StopAsync();
			}
			catch (Exception e)
			{
				_messageViewer.View("Error!", "Can't stop a server.");
				Log.Error(e, "Failed stop the server.");
			}
		}

		public event EventHandler CanExecuteChanged;
	}
}