using System;

namespace HttpMock.VisualServer
{
	internal class EnvironmentWrapper : IEnvironmentWrapper
	{
		public string GetRoamingPath()
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		}
	}
}