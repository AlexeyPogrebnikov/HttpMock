using System;
using NUnit.Framework;

namespace HttpMock.Core.Tests
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

		[Test]
		public void Add_throw_ArgumentException_if_Method_is_null()
		{
			var mockCache = new MockCache();
			var mock = new Mock { Method = null };

			var exception = Assert.Throws<ArgumentException>(() => mockCache.Add(mock));
			Assert.AreEqual("Method of the mock is null or empty.", exception.Message);
		}

		[Test]
		public void Add_throw_ArgumentException_if_StatusCode_is_empty()
		{
			var mockCache = new MockCache();
			var mock = new Mock
			{
				Method = "GET",
				StatusCode = ""
			};

			var exception = Assert.Throws<ArgumentException>(() => mockCache.Add(mock));
			Assert.AreEqual("StatusCode of the mock is null or empty.", exception.Message);
		}
	}
}