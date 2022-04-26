using System;
using System.IO;
using NUnit.Framework;

namespace HttpMock.Runner.Tests
{
	[TestFixture]
	internal class ServerValidatorTests
	{
		[Test]
		public void Validate_server_is_too_old()
		{
			string serverFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(serverFolder);

			string serverFileName = Path.Combine(serverFolder, "too_old_server.exe");
			ServerAssemblyGenerator.Generate(serverFileName, "0.4.0");

			Console.WriteLine(serverFileName);

			var versionService = new VersionServiceStub(new Version("0.5.0.0"));
			ServerValidator validator = new(versionService);

			var exception = Assert.Throws<InvalidOperationException>(() => validator.Validate(serverFileName));

			Assert.AreEqual(
				"The version of HttpMock.Runner and the version of the server are different. The version of HttpMock.Runner is 0.5.0.0. The version of server is 0.4.0.0. You can download the server here https://github.com/AlexeyPogrebnikov/HttpMock/releases",
				exception.Message);
		}

		[Test]
		public void Validate_server_version_is_0_7_1_lib_version_is_0_7_2()
		{
			string serverFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(serverFolder);

			string serverFileName = Path.Combine(serverFolder, "too_old_server.exe");
			ServerAssemblyGenerator.Generate(serverFileName, "0.7.1");

			Console.WriteLine(serverFileName);

			var versionService = new VersionServiceStub(new Version("0.7.2.0"));
			ServerValidator validator = new(versionService);

			validator.Validate(serverFileName);
		}

		private class VersionServiceStub : IVersionService
		{
			private readonly Version _version;

			public VersionServiceStub(Version version)
			{
				_version = version;
			}

			public Version GetVersion()
			{
				return _version;
			}
		}
	}
}