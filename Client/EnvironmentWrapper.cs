using System;

namespace HttpMock.Client
{
	internal class EnvironmentWrapper : IEnvironmentWrapper
	{
		public string GetRoamingPath()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		}
	}
}