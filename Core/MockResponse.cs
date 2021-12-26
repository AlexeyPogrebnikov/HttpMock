using System;
using System.Text.Json.Serialization;

namespace HttpMock.Core
{
	public class MockResponse
	{
		public static MockResponse CreateNew()
		{
			return new MockResponse
			{
				Uid = Guid.NewGuid(),
				Response = new Response()
			};
		}

		[JsonIgnore]
		public Guid Uid { get; set; }

		public string Method { get; set; }

		public string Path { get; set; }

		public Response Response { get; set; }

		public MockResponse Clone()
		{
			return new MockResponse
			{
				Uid = Uid,
				Method = Method,
				Path = Path,
				Response = Response.Clone()
			};
		}
	}
}