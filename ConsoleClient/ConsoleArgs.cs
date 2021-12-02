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
	}
}