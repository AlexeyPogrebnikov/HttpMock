using System;
using System.IO;
using Moq;
using NUnit.Framework;
using TcpMock.Client;
using Mock = TcpMock.Core.Mock;

namespace Client.Tests
{
	[TestFixture]
	public class WorkSessionSaverTests
	{
		[Test]
		public void Load_skip_null_mocks()
		{
			var environmentWrapper = new Mock<IEnvironmentWrapper>();
			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			environmentWrapper
				.Setup(environment => environment.GetRoamingPath())
				.Returns(testPath);

			var saver = new WorkSessionSaver(environmentWrapper.Object);

			var workSession = new WorkSession();
			workSession.Mocks = new[]
			{
				new Mock
				{
					Method = "GET",
					Path = "/foo",
					StatusCode = "200"
				},
				null
			};

			saver.Save(workSession);
			WorkSession loadedWorkSession = saver.Load();
			Assert.AreEqual(1, loadedWorkSession.Mocks.Length);
			Assert.AreEqual("GET", loadedWorkSession.Mocks[0].Method);
			Assert.AreEqual("/foo", loadedWorkSession.Mocks[0].Path);
			Assert.AreEqual("200", loadedWorkSession.Mocks[0].StatusCode);
		}

		[Test]
		public void Load_mocks_is_null()
		{
			var environmentWrapper = new Mock<IEnvironmentWrapper>();
			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			environmentWrapper
				.Setup(environment => environment.GetRoamingPath())
				.Returns(testPath);

			var saver = new WorkSessionSaver(environmentWrapper.Object);

			var workSession = new WorkSession();
			workSession.Mocks = null;

			saver.Save(workSession);
			WorkSession loadedWorkSession = saver.Load();
			Assert.AreEqual(0, loadedWorkSession.Mocks.Length);
		}
	}
}