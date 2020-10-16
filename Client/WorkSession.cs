using TcpMock.Core;

namespace TcpMock.Client
{
	public class WorkSession
	{
		public ConnectionSettings ConnectionSettings { get; set; }

		public Mock[] Mocks { get; set; }
	}
}