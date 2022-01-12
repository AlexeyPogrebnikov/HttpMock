using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using HttpMock.Core;

namespace HttpMock.VisualServer.Commands
{
	public class StartHttpServerCommand : ICommand
	{
		private readonly IHttpServer _httpServer;
		private readonly IMessageViewer _messageViewer;

		public StartHttpServerCommand(IHttpServer httpServer, IMessageViewer messageViewer)
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

			if (!TryParsePort(connectionSettings, out int? port))
				return;

			await Task.Run(() =>
			{
				try
				{
					_httpServer.Start(address, port.GetValueOrDefault());
				}
				catch
				{
					_messageViewer.View("Error!", "Can't start a server. Please check a host and a port.");
					//TODO log error
				}
			});
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

		private bool TryParsePort(ConnectionSettings connectionSettings, out int? port)
		{
			port = null;

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