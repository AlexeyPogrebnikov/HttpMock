using System;
using System.Windows.Input;
using HttpMock.Core;
using Microsoft.Win32;

namespace HttpMock.Client.Commands
{
	public class OpenCommand : ICommand
	{
		private readonly IMockCache _mockCache;

		public OpenCommand(IMockCache mockCache)
		{
			_mockCache = mockCache;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
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

				_mockCache.Clear();
				_mockCache.AddRange(project.Mocks);
				ServerProjectOpened?.Invoke(this, EventArgs.Empty);
			}
		}

		public event EventHandler CanExecuteChanged;

		internal event EventHandler ServerProjectOpened;
	}
}