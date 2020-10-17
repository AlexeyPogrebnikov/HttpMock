using NUnit.Framework;

namespace TcpMock.Core.Tests
{
	[TestFixture]
	public class RequestParserTests
	{
		[Test]
		public void Parse_GET_Slash()
		{
			var requestParser = new RequestParser();

			TcpInteraction tcpInteraction = requestParser.Parse("GET / HTTP/1.1");

			Assert.AreEqual("GET", tcpInteraction.Method);
			Assert.AreEqual("/", tcpInteraction.Path);
		}

		[Test]
		public void Parse_GET_Slash_favicon()
		{
			var requestParser = new RequestParser();

			TcpInteraction tcpInteraction = requestParser.Parse("POST /favicon.ico HTTP/1.1");

			Assert.AreEqual("POST", tcpInteraction.Method);
			Assert.AreEqual("/favicon.ico", tcpInteraction.Path);
		}
	}
}