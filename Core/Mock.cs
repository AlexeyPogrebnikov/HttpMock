using System;

namespace TcpMock.Core
{
	public class Mock
	{
		public Guid Uid { get; set; }

		public string Method { get; set; }

		public string Path { get; set; }

		public string StatusCode { get; set; }

		public string Content { get; set; }
	}
}