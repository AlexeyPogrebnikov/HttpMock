using System;

namespace HttpMock.Core
{
	public class TcpInteraction
	{
		public Guid Uid { get; set; }

		public TimeSpan Time { get; set; }

		public bool Handled { get; set; }

		public string Path { get; set; }

		public string Method { get; set; }

		public string StatusCode { get; set; }

		public string RawRequest { get; set; }
	}
}