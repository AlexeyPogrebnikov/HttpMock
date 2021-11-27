using System;
using System.IO;
using HttpMock.Core;
using NUnit.Framework;

namespace HttpMock.Client.Tests
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
					new Mock
					{
						Method = "GET",
						Path = "/"
					}
				}
			};

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			string fileName = Path.Combine(testPath, "project.json");

			project.Save(fileName);

			string expectedContent = GetType().Assembly
				.GetEmbeddedResourceTextContent("HttpMock.Client.Tests.expected_server_project_file_content.json");
			Assert.AreEqual(expectedContent, File.ReadAllText(fileName));
		}
	}
}