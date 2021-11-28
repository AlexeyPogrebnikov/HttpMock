using System;
using System.IO;
using System.Text.Json;
using HttpMock.Core;

namespace HttpMock.Client
{
	public class ServerProject
	{
		public string Host { get; set; }

		public string Port { get; set; }

		public Mock[] Mocks { get; set; }

		public void Save(string fileName)
		{
			var options = new JsonSerializerOptions
			{
				WriteIndented = true
			};
			string json = JsonSerializer.Serialize(this, options);

			File.WriteAllText(fileName, json);
		}

		public void Load(string fileName)
		{
			string json = File.ReadAllText(fileName);

			var project = JsonSerializer.Deserialize<ServerProject>(json);

			Host = project.Host;
			Port = project.Port;
			Mocks = project.Mocks;

			foreach (Mock mock in Mocks)
				mock.Uid = Guid.NewGuid();
		}
	}
}