using System.Text;
using NUnit.Framework;

namespace HttpMock.Core.Tests
{
	[TestFixture]
	public class ResponseBuilderTests
	{
		[Test]
		public void Build_return_response()
		{
			Encoding encoding = Encoding.UTF8;

			ResponseBuilder builder = new ResponseBuilder(encoding);

			builder.SetStatusCode("404");
			builder.SetContent("foo");

			byte[] data = builder.Build();
			string response = encoding.GetString(data);
			Assert.AreEqual("HTTP/1.1 404 OK\r\nContent-Length: 3\r\n\r\nfoo\r\n", response);
		}
		
		[Test]
		public void Build_return_response_if_content_is_null()
		{
			Encoding encoding = Encoding.UTF8;

			ResponseBuilder builder = new ResponseBuilder(encoding);

			builder.SetStatusCode("404");
			builder.SetContent(null);

			byte[] data = builder.Build();
			string response = encoding.GetString(data);
			Assert.AreEqual("HTTP/1.1 404 OK\r\nContent-Length: 0\r\n\r\n", response);
		}
	}
}