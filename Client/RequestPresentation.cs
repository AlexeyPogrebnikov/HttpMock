using System;

namespace TcpMock.Client
{
	public class RequestPresentation
	{
		public Guid Uid { get; set; }

		public TimeSpan Time { get; set; }

		public string Method { get; set; }

		public string Path { get; set; }
	}
}