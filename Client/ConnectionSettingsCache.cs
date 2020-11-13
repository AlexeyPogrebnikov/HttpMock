namespace HttpMock.Client
{
	public static class ConnectionSettingsCache
	{
		public static ConnectionSettings ConnectionSettings { get; private set; }

		public static void Init(ConnectionSettings connectionSettings)
		{
			ConnectionSettings = connectionSettings;
		}
	}
}