using NUnit.Framework;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HttpMock.Core.Tests
{
	[TestFixture]
	class HttpServerTests
	{
		HttpServer _server;

		[TearDown]
		public void TearDown()
		{
			_server.Stop();
		}

		[Test, Timeout(5000)]
		public void Request_short_path()
		{
			MockCache mockCache = new();
			mockCache.Add(new MockResponse()
			{
				Method = "GET",
				Path = "/language",
				Response = new Response
				{
					StatusCode = "200",
					Content = "C#"
				}
			});

			_server = new(mockCache, new HttpInteractionCache());

			Task.Run(() => _server.Start(System.Net.IPAddress.Parse("127.0.0.1"), 80));

			WaitReadyServer();

			WebClient webClient = new();
			var response = webClient.DownloadString("http://127.0.0.1/language");
			Assert.AreEqual("C#", response);
		}

		[Test, Timeout(5000)]
		public void Request_path_has_2000_symbols()
		{
			MockCache mockCache = new();
			string path = "/goo" + new string('0', 1994) + "le";
			mockCache.Add(new MockResponse()
			{
				Method = "GET",
				Path = path,
				Response = new Response
				{
					StatusCode = "200",
					Content = "google"
				}
			});

			_server = new(mockCache, new HttpInteractionCache());

			Task.Run(() => _server.Start(System.Net.IPAddress.Parse("127.0.0.1"), 80));
			WaitReadyServer();

			WebClient webClient = new();
			var response = webClient.DownloadString("http://127.0.0.1" + path);
			Assert.AreEqual("google", response);
		}

		private void WaitReadyServer()
		{
			while (!_server.IsStarted)
			{
				Thread.Sleep(100);
			}
		}
	}
}
