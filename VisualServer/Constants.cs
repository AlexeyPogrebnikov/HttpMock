using System.Collections.Generic;

namespace HttpMock.VisualServer
{
	public static class Constants
	{
		public static IEnumerable<string> Methods
		{
			get { return new[] { "GET", "POST", "PUT", "DELETE" }; }
		}

		public static IEnumerable<int> StatusCodes
		{
			get { return new[] { 200, 400, 401, 403, 404, 500 }; }
		}
	}
}