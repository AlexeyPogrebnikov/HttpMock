using HttpMock.Core;

namespace HttpMock.ConsoleClient
{
	public class ConsoleArgs
	{
		public string ServerProjectFileName { get; set; }

		public static ConsoleArgs Parse(string[] args)
		{
			return new ConsoleArgs
			{
				ServerProjectFileName = args[0]
			};
		}

		public ServerProject CreateServerProject()
		{
			var serverProject = new ServerProject();

			serverProject.Load(ServerProjectFileName);

			return serverProject;
		}
	}
}