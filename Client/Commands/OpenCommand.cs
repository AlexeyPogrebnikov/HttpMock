using System;
using System.Windows.Input;
using HttpMock.Core;
using Microsoft.Win32;

namespace HttpMock.Client.Commands
{
	public class OpenCommand : ICommand
	{
		private readonly IHttpServer _httpServer;
		private readonly IMessageViewer _messageViewer;
		private readonly IMockCache _mockCache;

		public OpenCommand(IMockCache mockCache, IHttpServer httpServer, IMessageViewer messageViewer)
		{
			_mockCache = mockCache;
			_httpServer = httpServer;
			_messageViewer = messageViewer;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			if (_httpServer.IsStarted)
			{
				_messageViewer.View("Can't open a file", "Please stop the server before open a file.");
				return;
			}

			var dialog = new OpenFileDialog
			{
				DefaultExt = ".json",
				Filter = "JSON files (.json)|*.json"
			};

			if (dialog.ShowDialog() == true)
			{
				var project = new ServerProject();

				project.Load(dialog.FileName);

				ConnectionSettings connectionSettings = ConnectionSettingsCache.ConnectionSettings;
				connectionSettings.Host = project.Host;
				connectionSettings.Port = project.Port;

				_mockCache.Init(project.Mocks);
				ServerProjectOpened?.Invoke(this, EventArgs.Empty);
			}
		}

		public event EventHandler CanExecuteChanged;

		internal event EventHandler ServerProjectOpened;
	}
}