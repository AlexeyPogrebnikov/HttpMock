using System;
using System.IO;
using NUnit.Framework;

namespace HttpMock.Core.Tests
{
	[TestFixture]
	public class ServerProjectTests
	{
		[Test]
		public void Save_creates_json_file()
		{
			var project = new ServerProject
			{
				Host = "127.0.0.1",
				Port = "443",
				Mocks = new[]
				{
					new MockResponse
					{
						Method = "GET",
						Path = "/",
						Response = new Response()
					}
				}
			};

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			string fileName = Path.Combine(testPath, "project.json");

			project.Save(fileName);

			string expectedContent = GetType().Assembly
				.GetEmbeddedResourceTextContent("HttpMock.Core.Tests.server_project_file_content.json");
			Assert.AreEqual(expectedContent, File.ReadAllText(fileName));
		}

		[Test]
		public void Load_from_json_file()
		{
			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			string json = GetType().Assembly
				.GetEmbeddedResourceTextContent("HttpMock.Core.Tests.server_project_file_content.json");

			string fileName = Path.Combine(testPath, "project.json");

			File.WriteAllText(fileName, json);

			var project = new ServerProject();

			project.Load(fileName);

			Assert.AreEqual("127.0.0.1", project.Host);
			Assert.AreEqual("443", project.Port);
			Assert.AreEqual(1, project.Mocks.Length);
			Assert.AreNotEqual(Guid.Empty, project.Mocks[0].Uid);
			Assert.AreEqual("GET", project.Mocks[0].Method);
			Assert.AreEqual("/", project.Mocks[0].Path);
		}
	}
}