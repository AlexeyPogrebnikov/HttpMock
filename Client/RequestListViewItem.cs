using System;

namespace TcpMock.Client
{
	public class RequestListViewItem
	{
		public TimeSpan Time { get; set; }

		public string Method { get; set; }

		public string Path { get; set; }
	}
}