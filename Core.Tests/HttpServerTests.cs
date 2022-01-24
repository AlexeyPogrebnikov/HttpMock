﻿using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HttpMock.Core.Tests
{
	[TestFixture]
	public class HttpServerTests
	{
		HttpServer _server;

		[SetUp]
		public void SetUp()
		{
			_server = new();
		}

		[TearDown]
		public void TearDown()
		{
			_server.Stop();
		}

		[Test, Timeout(5000)]
		public void Request_short_path()
		{
			_server.Routes.Add(new Route()
			{
				Method = "GET",
				Path = "/language",
				Response = new Response
				{
					StatusCode = 200,
					Body = "C#"
				}
			});

			Task.Run(() => _server.Start(IPAddress.Parse("127.0.0.1"), 80));

			WaitReadyServer();

			WebClient webClient = new();
			var response = webClient.DownloadString("http://127.0.0.1/language");
			Assert.AreEqual("C#", response);

			Assert.IsTrue(_server.IsStarted);

			Interaction[] interactions = _server.Interactions.PopAll().ToArray();
			Assert.AreEqual(1, interactions.Length);
			Assert.AreEqual(200, interactions[0].Response.StatusCode);
		}

		[Test, Timeout(5000)]
		public void Request_path_has_2000_symbols()
		{
			string path = "/goo" + new string('0', 1994) + "le";
			_server.Routes.Add(new Route()
			{
				Method = "GET",
				Path = path,
				Response = new Response
				{
					StatusCode = 200,
					Body = "google"
				}
			});

			Task.Run(() => _server.Start(IPAddress.Parse("127.0.0.1"), 80));
			WaitReadyServer();

			WebClient webClient = new();
			var response = webClient.DownloadString("http://127.0.0.1" + path);
			Assert.AreEqual("google", response);

			Assert.IsTrue(_server.IsStarted);

			Interaction[] interactions = _server.Interactions.PopAll().ToArray();
			Assert.AreEqual(1, interactions.Length);
			Assert.AreEqual(200, interactions[0].Response.StatusCode);
		}

		[Test]
		public void Start_throw_InvalidOperationException_if_server_already_started()
		{
			IPAddress address = IPAddress.Parse("127.0.0.1");
			Task.Run(() => _server.Start(address, 80));
			WaitReadyServer();
			
			InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => _server.Start(address, 5000));

			Assert.AreEqual("HTTP server is already started.", exception.Message);
		}

		[Test, Timeout(5000)]
		public void Response_not_found()
		{
			Task.Run(() => _server.Start(IPAddress.Parse("127.0.0.1"), 5000));

			WaitReadyServer();

			WebClient webClient = new();
			try
			{
				webClient.DownloadString("http://127.0.0.1:5000/year");
			}
			catch { }

			Assert.IsTrue(_server.IsStarted);

			Interaction[] interactions = _server.Interactions.PopAll().ToArray();

			Assert.AreEqual(1, interactions.Length);
			Assert.AreEqual(404, interactions[0].Response.StatusCode);
		}
		private void WaitReadyServer()
		{
			while (!_server.IsStarted)
			{
				Task.Delay(100).Wait();
			}
		}
	}
}
