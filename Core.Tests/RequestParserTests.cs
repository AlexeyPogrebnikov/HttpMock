using NUnit.Framework;

namespace HttpMock.Core.Tests
{
	[TestFixture]
	public class RequestParserTests
	{
		[Test]
		public void Parse_GET_Slash()
		{
			var requestParser = new RequestParser();

			Request request = requestParser.Parse("GET / HTTP/1.1");

			Assert.AreEqual("GET", request.Method);
			Assert.AreEqual("/", request.Path);
		}

		[Test]
		public void Parse_GET_Slash_favicon()
		{
			var requestParser = new RequestParser();

			Request request = requestParser.Parse("POST /favicon.ico HTTP/1.1");

			Assert.AreEqual("POST", request.Method);
			Assert.AreEqual("/favicon.ico", request.Path);
		}
	}
}