using System;
using System.IO;
using Moq;
using NUnit.Framework;
using Mock = HttpMock.Core.MockResponse;

namespace HttpMock.Client.Tests
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
				Mocks = new[]
				{
					new Mock
					{
						Method = "GET",
						Path = "/foo",
						StatusCode = "200"
					},
					null
				}
			};

			saver.Save(workSession);
			WorkSession loadedWorkSession = saver.Load();
			Assert.AreEqual(2, loadedWorkSession.Mocks.Length);
			Assert.AreEqual("GET", loadedWorkSession.Mocks[0].Method);
			Assert.AreEqual("/foo", loadedWorkSession.Mocks[0].Path);
			Assert.AreEqual("200", loadedWorkSession.Mocks[0].StatusCode);
			Assert.IsNull(loadedWorkSession.Mocks[1]);
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