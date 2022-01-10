using System;
using System.IO;
using Moq;
using NUnit.Framework;
using HttpMock.Core;
using HttpMock.Client;

namespace HttpMock.VisualServer.Tests
{
	[TestFixture]
	public class WorkSessionTests
	{
		[Test]
		public void Load_Routes_empty_if_json_file_does_not_exist()
		{
			var environmentWrapper = new Mock<IEnvironmentWrapper>();

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			environmentWrapper
				.Setup(environment => environment.GetRoamingPath())
				.Returns(testPath);

			var workSession = new WorkSession();
			workSession.Load(environmentWrapper.Object);

			Assert.IsEmpty(workSession.Routes);
			Assert.NotNull(workSession.ConnectionSettings);
		}

		[Test]
		public void Load_if_json_file_exists()
		{
			var environmentWrapper = new Mock<IEnvironmentWrapper>();

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			environmentWrapper
				.Setup(environment => environment.GetRoamingPath())
				.Returns(testPath);

			var workSession = new WorkSession
			{
				ConnectionSettings = new ConnectionSettings
				{
					Host = "127.0.0.1",
					Port = "80"
				},
				Routes = new[]
				{
					new Route
					{
						Path = "/",
						Method = "GET"
					}
				}
			};

			workSession.Save(environmentWrapper.Object);

			var loadedWorkSession = new WorkSession();
			loadedWorkSession.Load(environmentWrapper.Object);

			Assert.AreEqual("127.0.0.1", loadedWorkSession.ConnectionSettings.Host);
			Assert.AreEqual("80", loadedWorkSession.ConnectionSettings.Port);
			Assert.AreEqual(1, loadedWorkSession.Routes.Length);
			Assert.AreEqual("/", loadedWorkSession.Routes[0].Path);
			Assert.AreEqual("GET", loadedWorkSession.Routes[0].Method);
		}

		[Test]
		public void Load_if_json_file_exists_and_ConnectionSettings_is_null()
		{
			var environmentWrapper = new Mock<IEnvironmentWrapper>();

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			environmentWrapper
				.Setup(environment => environment.GetRoamingPath())
				.Returns(testPath);

			var workSession = new WorkSession
			{
				ConnectionSettings = null,
				Routes = new Route[]
				{
					null
				}
			};

			workSession.Save(environmentWrapper.Object);

			var loadedWorkSession = new WorkSession();
			loadedWorkSession.Load(environmentWrapper.Object);

			Assert.NotNull(loadedWorkSession.ConnectionSettings);
			Assert.IsEmpty(loadedWorkSession.Routes);
		}

		[Test]
		public void Load_if_Routes_is_null()
		{
			var environmentWrapper = new Mock<IEnvironmentWrapper>();

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			environmentWrapper
				.Setup(environment => environment.GetRoamingPath())
				.Returns(testPath);

			var workSession = new WorkSession
			{
				Routes = null
			};

			workSession.Save(environmentWrapper.Object);

			var loadedWorkSession = new WorkSession();
			loadedWorkSession.Load(environmentWrapper.Object);

			Assert.IsEmpty(loadedWorkSession.Routes);
		}
	}
}