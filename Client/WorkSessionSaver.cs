using System.IO;
using System.Linq;
using System.Text.Json;
using HttpMock.Core;

namespace HttpMock.Client
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
			string httpMockPath = GetHttpMockPath();

			if (!Directory.Exists(httpMockPath))
				Directory.CreateDirectory(httpMockPath);

			string workSessionFileName = GetWorkSessionFileName(httpMockPath);

			string json = JsonSerializer.Serialize(workSession);

			File.WriteAllText(workSessionFileName, json);
		}

		public WorkSession Load()
		{
			string httpMockPath = GetHttpMockPath();

			string workSessionFileName = GetWorkSessionFileName(httpMockPath);

			string json = File.ReadAllText(workSessionFileName);

			var workSession = JsonSerializer.Deserialize<WorkSession>(json);

			workSession.Mocks = workSession.Mocks == null
				? new Mock[0]
				: workSession.Mocks.Where(mock => mock != null).ToArray();

			return workSession;
		}

		private string GetHttpMockPath()
		{
			string roamingPath = _environmentWrapper.GetRoamingPath();

			string httpMockPath = Path.Combine(roamingPath, "HttpMock");

			return httpMockPath;
		}

		private static string GetWorkSessionFileName(string httpMockPath)
		{
			return Path.Combine(httpMockPath, "user_session.json");
		}
	}
}