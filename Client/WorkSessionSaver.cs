using System;
using System.IO;
using System.Text.Json;

namespace TcpMock.Client
{
	public class WorkSessionSaver
	{
		public void Save(WorkSession workSession)
		{
			string tcpMockPath = GetTcpMockPath();

			if (!Directory.Exists(tcpMockPath)) 
				Directory.CreateDirectory(tcpMockPath);

			string workSessionFileName = GetWorkSessionFileName(tcpMockPath);

			string json = JsonSerializer.Serialize(workSession);
			
			File.WriteAllText(workSessionFileName, json);
		}

		public WorkSession Load()
		{
			string tcpMockPath = GetTcpMockPath();

			string workSessionFileName = GetWorkSessionFileName(tcpMockPath);

			string json = File.ReadAllText(workSessionFileName);

			return JsonSerializer.Deserialize<WorkSession>(json);
		}

		private static string GetTcpMockPath()
		{
			string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			string tcpMockPath = Path.Combine(roamingPath, "TcpMock");

			return tcpMockPath;
		}

		private static string GetWorkSessionFileName(string tcpMockPath)
		{
			return Path.Combine(tcpMockPath, "user_session.json");
		}
	}
}