﻿using System;
using System.Linq;
using System.Windows.Input;
using HttpMock.Core;
using Microsoft.Win32;

namespace HttpMock.Client.Commands
{
	public class SaveAsCommand : ICommand
	{
		private readonly IHttpServer _httpServer;

		public SaveAsCommand(IHttpServer httpServer)
		{
			_httpServer = httpServer;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var dialog = new SaveFileDialog
			{
				FileName = "server_project",
				DefaultExt = ".json",
				Filter = "JSON files (.json)|*.json"
			};

			if (dialog.ShowDialog() == true)
			{
				ConnectionSettings connectionSettings = ConnectionSettingsCache.ConnectionSettings;

				var project = new ServerProject
				{
					Host = connectionSettings.Host,
					Port = connectionSettings.Port,
					Mocks = _httpServer.Routes.GetAll().ToArray()
				};

				project.Save(dialog.FileName);
			}
		}

		public event EventHandler CanExecuteChanged;
	}
}