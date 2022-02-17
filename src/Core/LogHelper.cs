using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;

namespace HttpMock.Core
{
	public static class LogHelper
	{
		private const string LogPattern = "log-.txt";

		public static void Init(bool writeToConsole)
		{
			using IHost host = Host.CreateDefaultBuilder().Build();

			IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

			string logDir = config.GetValue<string>("LogDir");

			string path = null;
			if (string.IsNullOrEmpty(logDir))
				path = LogPattern;
			else
				path = Path.Combine(logDir, LogPattern);

			LoggerConfiguration configuration = new LoggerConfiguration()
							.MinimumLevel.Information()
							.WriteTo.File(path, rollingInterval: RollingInterval.Day);

			if (writeToConsole)
				configuration.WriteTo.Console();

			Log.Logger = configuration.CreateLogger();
		}
	}
}
