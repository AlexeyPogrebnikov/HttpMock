﻿using System.Collections.Generic;

namespace HttpMock.Client
{
	public static class Constants
	{
		public static IEnumerable<string> Methods
		{
			get { return new[] { "GET", "POST", "PUT", "DELETE" }; }
		}

		public static IEnumerable<string> StatusCodes
		{
			get { return new[] { "200", "400", "401", "403", "404", "500" }; }
		}

		public const int MaxMocksCountInFreeVersion = 3;
	}
}