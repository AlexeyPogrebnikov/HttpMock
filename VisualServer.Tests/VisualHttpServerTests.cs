using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HttpMock.Core;
using NUnit.Framework;

namespace HttpMock.VisualServer.Tests
{
	[TestFixture]
	public class VisualHttpServerTests
	{
		[SetUp]
		public void SetUp()
		{
			_server = new VisualHttpServer();
		}

		[TearDown]
		public void TearDown()
		{
			if (_server.IsStarted)
				_server.StopAsync().Wait();
		}

		private VisualHttpServer _server;

		[Test]
		[Timeout(5000)]
		public void Request_short_path()
		{
			_server.Routes.Init(new[]
			{
				new Route
				{
					Method = "GET",
					Path = "/language",
					Response = new Response
					{
						StatusCode = 200,
						Body = "C#"
					}
				}
			});

			Task.Run(() => _server.StartAsync(IPAddress.Parse("127.0.0.1"), 80));

			WaitReadyServer();

			WebClient webClient = new();
			string response = webClient.DownloadString("http://127.0.0.1/language");
			Assert.AreEqual("C#", response);

			Assert.IsTrue(_server.IsStarted);

			var interactions = _server.Interactions.PopAll().ToArray();
			Assert.AreEqual(1, interactions.Length);
			Assert.AreEqual(200, interactions[0].Response.StatusCode);
		}

		[Test]
		[Timeout(5000)]
		public void Request_path_has_2000_symbols()
		{
			string path = "/goo" + new string('0', 1994) + "le";
			_server.Routes.Init(new[]
			{
				new Route
				{
					Method = "GET",
					Path = path,
					Response = new Response
					{
						StatusCode = 200,
						Body = "google"
					}
				}
			});

			Task.Run(() => _server.StartAsync(IPAddress.Parse("127.0.0.1"), 80));
			WaitReadyServer();

			WebClient webClient = new();
			string response = webClient.DownloadString("http://127.0.0.1" + path);
			Assert.AreEqual("google", response);

			Assert.IsTrue(_server.IsStarted);

			var interactions = _server.Interactions.PopAll().ToArray();
			Assert.AreEqual(1, interactions.Length);
			Assert.AreEqual(200, interactions[0].Response.StatusCode);
		}

		[Test]
		[Timeout(5000)]
		public void StartAsync_throw_InvalidOperationException_if_server_already_started()
		{
			var address = IPAddress.Parse("127.0.0.1");
			Task.Run(() => _server.StartAsync(address, 80));
			WaitReadyServer();

			var exception = Assert.ThrowsAsync<InvalidOperationException>(() => _server.StartAsync(address, 5000));

			Assert.AreEqual("HTTP server is already started.", exception.Message);
		}

		[Test]
		[Timeout(5000)]
		public void Response_not_found()
		{
			Task.Run(() => _server.StartAsync(IPAddress.Parse("127.0.0.1"), 5000));

			WaitReadyServer();

			WebClient webClient = new();
			try
			{
				webClient.DownloadString("http://127.0.0.1:5000/year");
			}
			catch
			{
			}

			Assert.IsTrue(_server.IsStarted);

			var interactions = _server.Interactions.PopAll().ToArray();

			Assert.AreEqual(1, interactions.Length);
			Assert.AreEqual(404, interactions[0].Response.StatusCode);
		}

		private void WaitReadyServer()
		{
			while (!_server.IsStarted) Task.Delay(100).Wait();
		}

		[Test]
		[Timeout(5000)]
		public void StopAsync_throw_InvalidOperationException_if_server_is_stopped()
		{
			var exception = Assert.ThrowsAsync<InvalidOperationException>(() => _server.StopAsync());

			Assert.AreEqual("HTTP server is stopped.", exception.Message);
		}
	}
}