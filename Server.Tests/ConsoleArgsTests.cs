using HttpMock.ConsoleClient;
using NUnit.Framework;
using System;
using System.Net;

namespace HttpMock.Server.Tests
{
	[TestFixture]
	public class ConsoleArgsTests
	{
		[Test]
		public void Ctor_take_ServerProjectFileName_from_first_item()
		{
			ConsoleArgs args = new(new[] { "C:\\foo.json" });

			Assert.AreEqual("C:\\foo.json", args.ServerProjectFileName);
		}

		[Test]
		public void Ctor_take_host_after_host_keyword()
		{
			ConsoleArgs args = new(new[] { "config.json", "-host", "192.158.1.38" });

			Assert.AreEqual(IPAddress.Parse("192.158.1.38"), args.Host);
		}

		[Test]
		public void Ctor_args_is_empty()
		{
			ConsoleArgs args = new(Array.Empty<string>());

			Assert.IsNull(args.ServerProjectFileName);
		}

		[Test]
		public void Ctor_args_contains_only_host()
		{
			ConsoleArgs args = new(new[] { "-host", "127.0.0.1" });

			Assert.IsNull(args.ServerProjectFileName);
			Assert.AreEqual(IPAddress.Parse("127.0.0.1"), args.Host);
		}

		[Test]
		public void Ctor_args_contains_only_port()
		{
			ConsoleArgs args = new(new[] { "-port", "80" });

			Assert.IsNull(args.ServerProjectFileName);
			Assert.AreEqual(80, args.Port);
		}

		[Test]
		public void Ctor_throw_InvalidConsoleArgsException_if_ServerProjectFileName_is_duplicated()
		{
			InvalidConsoleArgsException exception = Assert.Throws<InvalidConsoleArgsException>(() => new ConsoleArgs(new[] { "1.json", "2.json" }));

			Assert.AreEqual("The path to the file with server settings must be specified only once. Path1: '1.json', Path2: '2.json'.", exception.Message);
		}

		[Test]
		public void Ctor_throw_InvalidConsoleArgsException_if_host_is_duplicated()
		{
			string[] args = new[] { "-host", "192.168.10.138", "-host", "127.0.0.1" };
			InvalidConsoleArgsException exception = Assert.Throws<InvalidConsoleArgsException>(() => new ConsoleArgs(args));

			Assert.AreEqual("The host parameter must be specified only once. Host1: '192.168.10.138', Host2: '127.0.0.1'.", exception.Message);
		}

		[Test]
		public void Ctor_throw_InvalidConsoleArgsException_if_port_is_duplicated()
		{
			string[] args = new[] { "-port", "80", "-port", "443" };
			InvalidConsoleArgsException exception = Assert.Throws<InvalidConsoleArgsException>(() => new ConsoleArgs(args));

			Assert.AreEqual("The port parameter must be specified only once. Port1: '80', Port2: '443'.", exception.Message);
		}

		[Test]
		public void Ctor_host_is_omitted()
		{
			InvalidConsoleArgsException exception = Assert.Throws<InvalidConsoleArgsException>(() => new ConsoleArgs(new[] { "-host" }));

			Assert.AreEqual("After the -host keyword an IP address must be specified (e.g., -host 127.0.0.1).", exception.Message);
		}

		[Test]
		public void Ctor_port_is_omitted()
		{
			InvalidConsoleArgsException exception = Assert.Throws<InvalidConsoleArgsException>(() => new ConsoleArgs(new[] { "-port" }));

			Assert.AreEqual("After the -port keyword a number must be specified (e.g., -port 80).", exception.Message);
		}
	}
}