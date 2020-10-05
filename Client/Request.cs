using System;

namespace TcpMock.Client
{
	public class Request
	{
		public TimeSpan Time { get; set; }

		public string Url { get; set; }
	}
}