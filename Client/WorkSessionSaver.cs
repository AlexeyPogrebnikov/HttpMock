using System.IO;
using System.Linq;
using System.Text.Json;
using TcpMock.Core;

namespace TcpMock.Client
{
	public class WorkSessionSaver
	{
		private readonly IEnvironmentWrapper _environmentWrapper;

		public WorkSessionSaver(IEnvironmentWrapper environmentWrapper)
		{
			_environmentWrapper = environmentWrapper;
		}

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

			var workSession = JsonSerializer.Deserialize<WorkSession>(json);

			workSession.Mocks = workSession.Mocks == null
				? new Mock[0]
				: workSession.Mocks.Where(mock => mock != null).ToArray();

			return workSession;
		}

		private string GetTcpMockPath()
		{
			string roamingPath = _environmentWrapper.GetRoamingPath();

			string tcpMockPath = Path.Combine(roamingPath, "TcpMock");

			return tcpMockPath;
		}

		private static string GetWorkSessionFileName(string tcpMockPath)
		{
			return Path.Combine(tcpMockPath, "user_session.json");
		}
	}
}