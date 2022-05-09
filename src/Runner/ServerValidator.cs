using System;
using System.Diagnostics;
using System.IO;

namespace HttpMock.Runner
{
	internal class ServerValidator
	{
		private readonly IVersionService _versionService;

		public ServerValidator(IVersionService versionService)
		{
			_versionService = versionService;
		}

		/// <param name="serverFileName">The path to the server file.</param>
		/// <exception cref="FileNotFoundException">The server not found.</exception>
		/// <exception cref="InvalidOperationException">The library version does not match the server version.</exception>
		public void Validate(string serverFileName)
		{
			if (!File.Exists(serverFileName))
				throw new FileNotFoundException(
					"Server executable file not found. You can download the server here https://github.com/AlexeyPogrebnikov/HttpMock/releases",
					serverFileName);

			string fileVersion = FileVersionInfo.GetVersionInfo(serverFileName).FileVersion;
			var serverVersion = new Version(fileVersion);
			Version version = _versionService.GetVersion();

			if (version.Major != serverVersion.Major || version.Minor != serverVersion.Minor)
			{
				string assemblyName = GetType().Assembly.GetName().Name;

				string message = $"The version of {assemblyName} and the version of the server are different. " +
				                 $"The version of {assemblyName} is {version}. The version of server is {fileVersion}. " +
				                 "You can download the server here https://github.com/AlexeyPogrebnikov/HttpMock/releases";

				throw new InvalidOperationException(message);
			}
		}
	}
}