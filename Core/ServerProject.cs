using System.IO;
using System.Text.Json;

namespace HttpMock.Core
{
	public class ServerProject
	{
		public Connection Connection { get; set; }

		public Route[] Routes { get; set; }

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

			Connection = project.Connection;
			Routes = project.Routes;
		}
	}
}