using System;
using System.IO;
using Moq;
using NUnit.Framework;
using Mock = HttpMock.Core.Route;

namespace HttpMock.VisualServer.Tests
{
	[TestFixture]
	public class WorkSessionSaverTests
	{
		[Test]
		public void Save_Load()
		{
			var environmentWrapper = new Mock<IEnvironmentWrapper>();
			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			environmentWrapper
				.Setup(environment => environment.GetRoamingPath())
				.Returns(testPath);

			var saver = new WorkSessionSaver(environmentWrapper.Object);

			var workSession = new WorkSession
			{
				Routes = new[]
				{
					new Mock
					{
						Method = "GET",
						Path = "/foo",
						Response = new Core.Response
						{
							StatusCode = 200
						}
					},
					null
				}
			};

			saver.Save(workSession);
			WorkSession loadedWorkSession = saver.Load();
			Assert.AreEqual(2, loadedWorkSession.Routes.Length);
			Assert.AreEqual("GET", loadedWorkSession.Routes[0].Method);
			Assert.AreEqual("/foo", loadedWorkSession.Routes[0].Path);
			Assert.AreEqual(200, loadedWorkSession.Routes[0].Response.StatusCode);
			Assert.IsNull(loadedWorkSession.Routes[1]);
		}

		[Test]
		public void Load_return_null_if_httpMockPath_does_not_exist()
		{
			var environmentWrapper = new Mock<IEnvironmentWrapper>();

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			environmentWrapper
				.Setup(environment => environment.GetRoamingPath())
				.Returns(testPath);

			var saver = new WorkSessionSaver(environmentWrapper.Object);
			WorkSession loadedWorkSession = saver.Load();

			Assert.IsNull(loadedWorkSession);
		}
	}
}