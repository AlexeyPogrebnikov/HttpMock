using System.IO;
using System.Text.Json;

namespace HttpMock.Client
{
	internal class WorkSessionSaver
	{
		private readonly IEnvironmentWrapper _environmentWrapper;

		internal WorkSessionSaver(IEnvironmentWrapper environmentWrapper)
		{
			_environmentWrapper = environmentWrapper;
		}

		internal void Save(WorkSession workSession)
		{
			string httpMockPath = GetHttpMockPath();

			if (!Directory.Exists(httpMockPath))
				Directory.CreateDirectory(httpMockPath);

			string workSessionFileName = GetWorkSessionFileName(httpMockPath);

			string json = JsonSerializer.Serialize(workSession);

			File.WriteAllText(workSessionFileName, json);
		}

		internal WorkSession Load()
		{
			string httpMockPath = GetHttpMockPath();

			if (!Directory.Exists(httpMockPath))
				Directory.CreateDirectory(httpMockPath);

			string workSessionFileName = GetWorkSessionFileName(httpMockPath);

			if (!File.Exists(workSessionFileName))
				return null;

			string json = File.ReadAllText(workSessionFileName);

			return JsonSerializer.Deserialize<WorkSession>(json);
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