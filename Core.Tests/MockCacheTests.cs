using System;
using System.Linq;
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

		[Test]
		public void AddRange_throw_ArgumentNullException_if_mock_is_null()
		{
			var mockCache = new MockCache();
			var mocks = new Mock[] { null };
			Assert.Throws<ArgumentNullException>(() => mockCache.AddRange(mocks));
		}

		[Test]
		public void GetAll_return_all_added_mocks()
		{
			var mockCache = new MockCache();
			var mock = new Mock { Method = "GET", StatusCode = "200" };
			mockCache.Add(mock);

			Mock[] mocks = mockCache.GetAll().ToArray();

			Assert.AreEqual(1, mocks.Length);
			Assert.AreSame(mock, mocks[0]);
		}

		[Test]
		public void Add_throw_InvalidOperationException_if_mock_with_same_Method_and_Path_already_exists()
		{
			var mockCache = new MockCache();
			var firstMock = new Mock
			{
				Method = "GET",
				StatusCode = "200",
				Path = "/"
			};

			mockCache.Add(firstMock);

			var secondMock = new Mock
			{
				Method = "GET",
				StatusCode = "500",
				Path = "/"
			};

			var exception = Assert.Throws<InvalidOperationException>(() => mockCache.Add(secondMock));
			Assert.AreEqual("Mock with same Method and Path already exists.", exception.Message);
		}

		[Test]
		public void Add_not_throw_exception_if_Paths_are_not_same()
		{
			var mockCache = new MockCache();
			var firstMock = new Mock
			{
				Method = "GET",
				StatusCode = "200",
				Path = "/"
			};

			mockCache.Add(firstMock);

			var secondMock = new Mock
			{
				Method = "GET",
				StatusCode = "500",
				Path = "/s"
			};

			mockCache.Add(secondMock);
		}
	}
}