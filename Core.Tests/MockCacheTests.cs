using System;
using NUnit.Framework;

namespace TcpMock.Core.Tests
{
	[TestFixture]
	public class MockCacheTests
	{
		[Test]
		public void Add_throw_ArgumentNullException_if_mock_is_null()
		{
			var mockCache = new MockCache();
			Assert.Throws<ArgumentNullException>(() => mockCache.Add(null));
		}
	}
}