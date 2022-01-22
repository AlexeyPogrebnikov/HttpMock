using System;

namespace HttpMock.Core
{
	public class HttpInteraction
	{
		public TimeSpan Time { get; set; }

		public bool Handled { get; set; }

		public string Path { get; set; }

		public string Method { get; set; }

		public int StatusCode { get; set; }

		public string RawRequest { get; set; }
	}
}