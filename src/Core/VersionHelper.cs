using System;
using System.Reflection;

namespace HttpMock.Core
{
	public static class VersionHelper
	{
		public static string GetCurrentAppVersion()
		{
			Version version = Assembly.GetEntryAssembly()?.GetName().Version;
			return version is { } ? version.ToString(2) : string.Empty;
		}
	}
}