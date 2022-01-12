using System;
using System.Collections.Generic;
using HttpMock.Core;

namespace HttpMock.Server
{
	public class HttpInteractionCacheLogger : IHttpInteractionCache
	{
		public void Add(HttpInteraction httpInteraction)
		{
			ConsoleColor defaultColor = Console.ForegroundColor;
			if (!httpInteraction.Handled)
				Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(
				$"{httpInteraction.Time} {httpInteraction.Method} {httpInteraction.Path} {httpInteraction.StatusCode}");
			Console.ForegroundColor = defaultColor;
		}

		public IEnumerable<HttpInteraction> PopAll()
		{
			throw new NotSupportedException();
		}
	}
}