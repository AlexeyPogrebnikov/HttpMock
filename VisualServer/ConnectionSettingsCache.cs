namespace HttpMock.VisualServer
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