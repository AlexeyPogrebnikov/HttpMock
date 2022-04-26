using System;

namespace HttpMock.Runner
{
	internal class VersionService : IVersionService
	{
		public Version GetVersion()
		{
			return GetType().Assembly.GetName().Version;
		}
	}
}