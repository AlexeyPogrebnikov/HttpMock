using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NUnit.Framework;

namespace HttpMock.Core.Tests
{
	[TestFixture]
	public class RequestTests
	{
		[SetUp]
		public void SetUp()
		{
			_listener = new TcpListener(IPAddress.Loopback, 5000);
			_listener.Start();
		}

		[TearDown]
		public void TearDown()
		{
			_listener.Stop();
		}

		private TcpListener _listener;

		[Test]
		[Timeout(5000)]
		public void Read_GET_slash()
		{
			Task.Run(async () =>
			{
				await Task.Delay(1000);

				var webClient = new WebClient();
				webClient.DownloadString("http://127.0.0.1:5000/");
			});

			using TcpClient client = _listener.AcceptTcpClient();
			using NetworkStream stream = client.GetStream();

			Request request = Request.Read(stream);

			Assert.AreEqual("GET", request.Method);
			Assert.AreEqual("/", request.Path);
		}

		[Test]
		[Timeout(5000)]
		public void Read_GET_slash_favicon()
		{
			Task.Run(async () =>
			{
				await Task.Delay(1000);

				var webClient = new WebClient();
				webClient.DownloadString("http://127.0.0.1:5000/favicon.ico");
			});

			using TcpClient client = _listener.AcceptTcpClient();
			using NetworkStream stream = client.GetStream();

			Request request = Request.Read(stream);

			Assert.AreEqual("GET", request.Method);
			Assert.AreEqual("/favicon.ico", request.Path);
		}

		[Test]
		[Timeout(5000)]
		public void Read_GET_slash_long_path()
		{
			string path = "/" + new string('a', 500) + new string('b', 500) + new string('c', 500) +
			              new string('d', 500);
			Task.Run(async () =>
			{
				await Task.Delay(1000);

				var webClient = new WebClient();
				webClient.DownloadString("http://127.0.0.1:5000" + path);
			});

			using TcpClient client = _listener.AcceptTcpClient();
			using NetworkStream stream = client.GetStream();

			Request request = Request.Read(stream);

			Assert.AreEqual("GET", request.Method);
			Assert.AreEqual(path, request.Path);
		}
	}
}