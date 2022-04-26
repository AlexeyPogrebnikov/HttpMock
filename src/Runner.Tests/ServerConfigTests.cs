using System;
using System.IO;
using NUnit.Framework;

namespace HttpMock.Runner.Tests
{
	[TestFixture]
	internal class ServerConfigTests
	{
		[Test]
		public void Save_config()
		{
			ServerConfig config = new()
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
						Path = "/order",
						Response = new Response
						{
							StatusCode = 200,
							Body = "abc"
						}
					}
				}
			};

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			string fileName = Path.Combine(testPath, "config.json");

			config.Save(fileName);

			string expectedContent = GetType().Assembly
				.GetEmbeddedResourceTextContent("HttpMock.Runner.Tests.server-config.json");

			Assert.AreEqual(expectedContent, File.ReadAllText(fileName));
		}
	}
}