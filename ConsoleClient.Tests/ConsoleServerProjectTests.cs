using HttpMock.Core;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;

namespace HttpMock.ConsoleClient.Tests
{
	[TestFixture]
	class ConsoleServerProjectTests
	{
		Mock<IHttpServer> _httpServer;

		[SetUp]
		public void SetUp()
		{
			_httpServer = new();

			_httpServer.SetupGet(server => server.Routes).Returns(new MockCache());
		}

		[Test]
		public void StartServer_start_server_with_parameters_from_serverProject_if_path_is_specified()
		{
			ServerProject serverProject = new()
			{
				Host = "127.0.0.1",
				Port = "80",
				Mocks = Array.Empty<Core.Route>()
			};

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			string fileName = Path.Combine(testPath, "project.json");
			serverProject.Save(fileName);

			string[] args = new[] { fileName };
			ConsoleArgs consoleArgs = new(args);

			ConsoleServerProject project = new(consoleArgs, _httpServer.Object);

			project.StartServer();

			_httpServer.Verify(server => server.Start(It.Is<IPAddress>(ip => ip.ToString() == "127.0.0.1"), It.Is<int>(port => port == 80)));
		}

		[Test]
		public void StartServer_start_server_with_parameters_from_console_args_if_path_is_not_specified()
		{
			string[] args = new[] { "-host", "192.88.99.1", "-port", "443" };
			ConsoleArgs consoleArgs = new(args);

			ConsoleServerProject project = new(consoleArgs, _httpServer.Object);

			project.StartServer();

			_httpServer.Verify(server => server.Start(It.Is<IPAddress>(ip => ip.ToString() == "192.88.99.1"), It.Is<int>(port => port == 443)));
		}

		[Test]
		public void StartServer_takes_host_from_console_args()
		{
			ServerProject serverProject = new()
			{
				Host = "127.0.0.1",
				Port = "80",
				Mocks = Array.Empty<Core.Route>()
			};

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			string fileName = Path.Combine(testPath, "project.json");
			serverProject.Save(fileName);

			string[] args = new[] { fileName, "-host", "192.168.1.100" };
			ConsoleArgs consoleArgs = new(args);

			ConsoleServerProject project = new(consoleArgs, _httpServer.Object);

			project.StartServer();

			_httpServer.Verify(server => server.Start(It.Is<IPAddress>(ip => ip.ToString() == "192.168.1.100"), It.Is<int>(port => port == 80)));
		}

		[Test]
		public void StartServer_takes_port_from_console_args()
		{
			ServerProject serverProject = new()
			{
				Host = "127.0.0.1",
				Port = "80",
				Mocks = Array.Empty<Core.Route>()
			};

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			string fileName = Path.Combine(testPath, "project.json");
			serverProject.Save(fileName);

			string[] args = new[] { fileName, "-port", "22" };
			ConsoleArgs consoleArgs = new(args);

			ConsoleServerProject project = new(consoleArgs, _httpServer.Object);

			project.StartServer();

			_httpServer.Verify(server => server.Start(It.Is<IPAddress>(ip => ip.ToString() == "127.0.0.1"), It.Is<int>(port => port == 22)));
		}
	}
}
