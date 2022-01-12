using HttpMock.Core;
using System.Net;

namespace HttpMock.Server
{
	public class ConsoleArgs
	{
		public string ServerProjectFileName { get; private set; }
		public IPAddress Host { get; private set; }
		public int? Port { get; private set; }
		private string hostAsStr;
		private string portAsStr;

		public ConsoleArgs(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				string arg = args[i];
				
				if (arg == "-host")
				{
					string nextArg = GetNextArg(args, i);
					if (string.IsNullOrWhiteSpace(nextArg))
						throw new InvalidConsoleArgsException("After the -host keyword an IP address must be specified (e.g., -host 127.0.0.1).");
					TryParseHost(nextArg);
					i++;
					continue;
				}

				if (arg == "-port")
				{
					string nextArg = GetNextArg(args, i);
					if (string.IsNullOrWhiteSpace(nextArg))
						throw new InvalidConsoleArgsException("After the -port keyword a number must be specified (e.g., -port 80).");
					TryParsePort(nextArg);
					i++;
					continue;
				}

				if (ServerProjectFileName == default)
					ServerProjectFileName = arg;
				else
					throw new InvalidConsoleArgsException($"The path to the file with server settings must be specified only once. Path1: '{ServerProjectFileName}', Path2: '{arg}'.");
			}
		}

		private static string GetNextArg(string[] args, int index)
		{
			if (index + 1 >= args.Length)
				return null;
			return args[index + 1];
		}

		private void TryParseHost(string arg)
		{
			if (Host == default)
			{
				Host = IPAddress.Parse(arg);
				hostAsStr = arg;
			}
			else
				throw new InvalidConsoleArgsException($"The host parameter must be specified only once. Host1: '{hostAsStr}', Host2: '{arg}'.");
		}

		private void TryParsePort(string arg)
		{
			if (Port == default)
			{
				Port = int.Parse(arg);
				portAsStr = arg;
			}
			else
				throw new InvalidConsoleArgsException($"The port parameter must be specified only once. Port1: '{portAsStr}', Port2: '{arg}'.");
		}

		//TODO move to ConsoleServerProject
		public ServerProject CreateServerProject()
		{
			var serverProject = new ServerProject();

			serverProject.Load(ServerProjectFileName);

			return serverProject;
		}
	}
}