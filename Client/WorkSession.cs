using HttpMock.Core;

namespace HttpMock.Client
{
	public class WorkSession
	{
		public ConnectionSettings ConnectionSettings { get; set; }

		public Mock[] Mocks { get; set; }
	}
}