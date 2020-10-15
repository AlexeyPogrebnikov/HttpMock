using System;

namespace TcpMock.Core
{
	public class Request
	{
		public TimeSpan Time { get; set; }

		public bool Handled { get; set; }

		public string Path { get; set; }

		public string Method { get; set; }
	}
}