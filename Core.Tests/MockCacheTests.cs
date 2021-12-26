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
			var mock = new MockResponse { Method = null };

			var exception = Assert.Throws<ArgumentException>(() => mockCache.Add(mock));
			Assert.AreEqual("Method of the route is null or empty.", exception.Message);
		}

		[Test]
		public void Add_throw_ArgumentException_if_StatusCode_is_empty()
		{
			var mockCache = new MockCache();
			var mock = new MockResponse
			{
				Method = "GET",
				Path = "/",
				Response = new Response
				{
					StatusCode = ""
				}
			};

			var exception = Assert.Throws<ArgumentException>(() => mockCache.Add(mock));
			Assert.AreEqual("StatusCode of the response is null or empty.", exception.Message);
		}

		[Test]
		public void Init_clear_previous_mocks()
		{
			MockCache mockCache = new();

			mockCache.Add(new MockResponse
			{
				Method = "GET",
				Path = "/",
				Response = new Response
				{
					StatusCode = "200"
				}
			});

			var mocks = new MockResponse[] {
				new MockResponse() 
				{ 
					Method = "GET",
					Path = "/clients",
					Response = new Response 
					{ 
						StatusCode = "200"
					}					
				},
				new MockResponse()
				{
					Method = "GET",
					Path = "/orders",
					Response = new Response
					{
						StatusCode = "200"
					}
				}
			};

			mockCache.Init(mocks);

			Assert.AreEqual(2, mockCache.GetAll().Count());
		}

		[Test]
		public void Init_throw_ArgumentNullException_if_mock_is_null()
		{
			var mockCache = new MockCache();
			var mocks = new MockResponse[] { null };
			Assert.Throws<ArgumentNullException>(() => mockCache.Init(mocks));
		}

		[Test]
		public void GetAll_return_all_added_mocks()
		{
			var mockCache = new MockCache();
			var mock = new MockResponse 
			{ 
				Method = "GET", 
				Path = "/",
				Response = new Response
				{
					StatusCode = "200"
				}
			};
			mockCache.Add(mock);

			MockResponse[] mocks = mockCache.GetAll().ToArray();

			Assert.AreEqual(1, mocks.Length);
			Assert.AreSame(mock, mocks[0]);
		}

		[Test]
		public void Add_throw_InvalidOperationException_if_mock_with_same_Method_and_Path_already_exists()
		{
			var mockCache = new MockCache();
			var firstMock = new MockResponse
			{
				Method = "GET",
				Path = "/",
				Response = new Response
				{
					StatusCode = "200"
				}
			};

			mockCache.Add(firstMock);

			var secondMock = new MockResponse
			{
				Method = "GET",
				Path = "/",
				Response = new Response
				{
					StatusCode = "500"
				}
			};

			var exception = Assert.Throws<InvalidOperationException>(() => mockCache.Add(secondMock));
			Assert.AreEqual("Route with same Method and Path already exists.", exception.Message);
		}

		[Test]
		public void Add_not_throw_exception_if_Paths_are_not_same()
		{
			var mockCache = new MockCache();
			var firstMock = new MockResponse
			{
				Method = "GET",
				Path = "/",
				Response = new Response
				{
					StatusCode = "200"
				}
			};

			mockCache.Add(firstMock);

			var secondMock = new MockResponse
			{
				Method = "GET",
				Path = "/s",
				Response = new Response
				{
					StatusCode = "500"
				}
			};

			mockCache.Add(secondMock);
		}
	}
}