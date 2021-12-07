using NUnit.Framework;

namespace HttpMock.ConsoleClient.Tests
{
	[TestFixture]
	public class ConsoleArgsTests
	{
		[Test]
		public void Parse_get_ServerProjectFileName_from_first_item()
		{
			ConsoleArgs args = ConsoleArgs.Parse(new[] {"C:\\foo.json"});

			Assert.AreEqual("C:\\foo.json", args.ServerProjectFileName);
		}
	}
}