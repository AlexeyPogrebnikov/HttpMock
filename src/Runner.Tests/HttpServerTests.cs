using System.IO;
using System.Net;
using NUnit.Framework;

namespace HttpMock.Runner.Tests
{
	[TestFixture]
	public class HttpServerTests
	{
		[Test]
		public void Route_GET_request()
		{
			using HttpServer server = new(@"C:\HttpMockServer\HttpMock.Server.exe", new ServerValidator(new VersionService()));
			Response response = new()
			{
				StatusCode = 200,
				Body = "2022"
			};

			server.RouteGet("/year", response);

			server.StartOnLocal(5000);

			WebClient client = new();
			string s = client.DownloadString("http://127.0.0.1:5000/year");

			Assert.AreEqual("2022", s);
		}

		[Test]
		public void Start_throw_FileNotFoundException_if_server_does_not_exist()
		{
			using HttpServer server = new(@"server.exe");

			FileNotFoundException exception = Assert.Throws<FileNotFoundException>(() => server.Start("127.0.0.1", 5000));

			Assert.AreEqual("server.exe", exception.FileName);
			Assert.AreEqual("Server executable file not found. You can download the server here https://github.com/AlexeyPogrebnikov/HttpMock/releases", exception.Message);
		}
	}
}
