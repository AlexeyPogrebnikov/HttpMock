using System;
using System.Text.Json.Serialization;

namespace HttpMock.Core
{
	public class Route
	{
		public static Route CreateNew()
		{
			return new Route
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

		public Route Clone()
		{
			return new Route
			{
				Uid = Uid,
				Method = Method,
				Path = Path,
				Response = Response.Clone()
			};
		}
	}
}