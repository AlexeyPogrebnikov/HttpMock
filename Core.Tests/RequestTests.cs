using NUnit.Framework;

namespace HttpMock.Core.Tests
{
	[TestFixture]
	public class RequestTests
	{
		[Test]
		public void Parse_GET_slash()
		{
			Request request = Request.Parse("GET / HTTP/1.1");

			Assert.AreEqual("GET", request.Method);
			Assert.AreEqual("/", request.Path);
		}

		[Test]
		public void Parse_GET_slash_favicon()
		{
			Request request = Request.Parse("POST /favicon.ico HTTP/1.1");

			Assert.AreEqual("POST", request.Method);
			Assert.AreEqual("/favicon.ico", request.Path);
		}
	}
}