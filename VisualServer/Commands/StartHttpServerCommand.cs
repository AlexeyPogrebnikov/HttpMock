using System;
using System.Net;
using System.Windows.Input;
using Serilog;

namespace HttpMock.VisualServer.Commands
{
	public class StartHttpServerCommand : ICommand
	{
		private readonly IVisualHttpServer _httpServer;
		private readonly IMessageViewer _messageViewer;

		public StartHttpServerCommand(IVisualHttpServer httpServer, IMessageViewer messageViewer)
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
			var connectionSettings = (ConnectionSettings) parameter;

			if (!TryParseIpAddress(connectionSettings, out IPAddress address))
				return;

			if (!TryParsePort(connectionSettings, out int port))
				return;

			try
			{
				await _httpServer.StartAsync(address, port);
			}
			catch (Exception e)
			{
				_messageViewer.View("Error!", "Can't start a server. Please check a host and a port.");
				Log.Error(e, "Failed start a server or process a request.");
			}
		}

		public event EventHandler CanExecuteChanged;

		private bool TryParseIpAddress(ConnectionSettings connectionSettings, out IPAddress address)
		{
			address = null;

			string host = connectionSettings.Host;
			if (string.IsNullOrWhiteSpace(host))
			{
				_messageViewer.View("Warning!", "Please enter a host.");
				return false;
			}

			try
			{
				address = IPAddress.Parse(host);
				return true;
			}
			catch
			{
				_messageViewer.View("Warning!", $"The host '{host}' is invalid.");
				return false;
			}
		}

		private bool TryParsePort(ConnectionSettings connectionSettings, out int port)
		{
			port = 0;

			string portAsStr = connectionSettings.Port;
			if (string.IsNullOrWhiteSpace(portAsStr))
			{
				_messageViewer.View("Warning!", "Please enter a port.");
				return false;
			}

			try
			{
				port = int.Parse(portAsStr);
				return true;
			}
			catch
			{
				_messageViewer.View("Warning!", $"The port '{portAsStr}' is invalid.");
				return false;
			}
		}
	}
}