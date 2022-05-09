using System;
using System.IO;
using HttpMock.Server.Core;
using NUnit.Framework;

namespace HttpMock.Server.Tests.Core
{
	[TestFixture]
	public class ServerProjectTests
	{
		[Test]
		public void Save_creates_json_file()
		{
			var project = new ServerProject
			{
				Connection = new Connection
				{
					Host = "127.0.0.1",
					Port = 443
				},
				Routes = new[]
				{
					new Route
					{
						Method = "GET",
						Path = "/",
						Response = new Response
						{
							StatusCode = 302
						}
					}
				}
			};

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			string fileName = Path.Combine(testPath, "project.json");

			project.Save(fileName);

			string expectedContent = GetType().Assembly
				.GetEmbeddedResourceTextContent("HttpMock.Server.Tests.Core.server_project_file_content.json");
			Assert.AreEqual(expectedContent, File.ReadAllText(fileName));
		}

		[Test]
		public void Load_from_json_file()
		{
			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			string json = GetType().Assembly
				.GetEmbeddedResourceTextContent("HttpMock.Server.Tests.Core.server_project_file_content.json");

			string fileName = Path.Combine(testPath, "project.json");

			File.WriteAllText(fileName, json);

			var project = new ServerProject();

			project.Load(fileName);

			Assert.AreEqual("127.0.0.1", project.Connection.Host);
			Assert.AreEqual(443, project.Connection.Port);
			Assert.AreEqual(1, project.Routes.Length);
			Assert.AreEqual("GET", project.Routes[0].Method);
			Assert.AreEqual("/", project.Routes[0].Path);
		}
	}
}