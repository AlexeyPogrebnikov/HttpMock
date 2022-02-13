using System;
using System.IO;
using System.Linq;
using System.Net;
using HttpMock.Core;
using NUnit.Framework;

namespace HttpMock.Server.Tests
{
	[TestFixture]
	internal class ConsoleServerProjectTests
	{
		[Test]
		public void Ctor_takes_parameters_from_serverProject_if_path_is_specified()
		{
			ServerProject serverProject = new()
			{
				Connection = new Connection
				{
					Host = "127.0.0.1",
					Port = 80
				},
				Routes = new[] {new Route()}
			};

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			string fileName = Path.Combine(testPath, "project.json");
			serverProject.Save(fileName);

			string[] args = {fileName};
			ConsoleArgs consoleArgs = new(args);

			ConsoleServerProject project = new(consoleArgs);

			Assert.AreEqual(IPAddress.Parse("127.0.0.1"), project.Address);
			Assert.AreEqual(80, project.Port);
			Assert.AreEqual(1, project.Routes.Count());
		}

		[Test]
		public void Ctor_takes_host_and_port_from_console_args_if_path_is_not_specified()
		{
			string[] args = {"-host", "192.88.99.1", "-port", "443"};
			ConsoleArgs consoleArgs = new(args);

			ConsoleServerProject project = new(consoleArgs);

			Assert.AreEqual(IPAddress.Parse("192.88.99.1"), project.Address);
			Assert.AreEqual(443, project.Port);
		}

		[Test]
		public void Ctor_takes_host_from_console_args()
		{
			ServerProject serverProject = new()
			{
				Connection = new Connection
				{
					Host = "127.0.0.1",
					Port = 80
				},
				Routes = Array.Empty<Route>()
			};

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			string fileName = Path.Combine(testPath, "project.json");
			serverProject.Save(fileName);

			string[] args = {fileName, "-host", "192.168.1.100"};
			ConsoleArgs consoleArgs = new(args);

			ConsoleServerProject project = new(consoleArgs);

			Assert.AreEqual(IPAddress.Parse("192.168.1.100"), project.Address);
			Assert.AreEqual(80, project.Port);
		}

		[Test]
		public void Ctor_takes_port_from_console_args()
		{
			ServerProject serverProject = new()
			{
				Connection = new Connection
				{
					Host = "127.0.0.1",
					Port = 80
				},
				Routes = Array.Empty<Route>()
			};

			string testPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(testPath);

			string fileName = Path.Combine(testPath, "project.json");
			serverProject.Save(fileName);

			string[] args = {fileName, "-port", "22"};
			ConsoleArgs consoleArgs = new(args);

			ConsoleServerProject project = new(consoleArgs);

			Assert.AreEqual(IPAddress.Parse("127.0.0.1"), project.Address);
			Assert.AreEqual(22, project.Port);
		}
	}
}