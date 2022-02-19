﻿using System;
using System.Windows.Input;
using HttpMock.Core;
using Microsoft.Win32;

namespace HttpMock.VisualServer.Commands
{
	public class OpenCommand : ICommand
	{
		private readonly IVisualHttpServer _httpServer;
		private readonly IMessageViewer _messageViewer;

		public OpenCommand(IVisualHttpServer httpServer, IMessageViewer messageViewer)
		{
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
				connectionSettings.Host = project.Connection.Host;
				connectionSettings.Port = project.Connection.Port.ToString();

				_httpServer.Routes.Init(project.Routes);
				ServerProjectOpened?.Invoke(this, EventArgs.Empty);
			}
		}

		public event EventHandler CanExecuteChanged;

		internal event EventHandler ServerProjectOpened;
	}
}