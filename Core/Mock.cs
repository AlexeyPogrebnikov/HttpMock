using System;

namespace HttpMock.Core
{
	public class Mock
	{
		public static Mock CreateNew()
		{
			return new Mock
			{
				Uid = Guid.NewGuid()
			};
		}

		public Guid Uid { get; set; }

		public string Method { get; set; }

		public string Path { get; set; }

		public string StatusCode { get; set; }

		public string Content { get; set; }
	}
}