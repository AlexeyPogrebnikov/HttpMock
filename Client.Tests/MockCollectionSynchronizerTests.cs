using System;
using System.Collections.Generic;
using HttpMock.Core;
using NUnit.Framework;

namespace HttpMock.Client.Tests
{
	[TestFixture]
	public class MockCollectionSynchronizerTests
	{
		private MockCollectionSynchronizer _synchronizer;

		[SetUp]
		public void SetUp()
		{
			_synchronizer = new MockCollectionSynchronizer();
		}

		[Test]
		public void Synchronize_add_mock_to_target()
		{
			var mock = new MockResponse
			{
				Uid = new Guid("20037054-3CC0-4457-9686-2B8A8C8B5814")
			};

			IList<MockResponse> source = new[]
			{
				mock
			};

			IList<MockResponse> target = new List<MockResponse>();

			_synchronizer.Synchronize(source, target);

			Assert.AreEqual(1, target.Count);
			Assert.AreSame(mock, target[0]);
		}

		[Test]
		public void Synchronize_dont_add_existing_mock_to_target()
		{
			var uid = new Guid("261B98BB-6BC7-4809-96E4-194A14EE36C6");

			IList<MockResponse> source = new List<MockResponse>
			{
				new MockResponse
				{
					Uid = uid
				}
			};

			IList<MockResponse> target = new List<MockResponse>
			{
				new MockResponse
				{
					Uid = uid
				}
			};

			_synchronizer.Synchronize(source, target);

			Assert.AreEqual(1, target.Count);
		}

		[Test]
		public void Synchronize_remove_mock_from_target_if_its_does_not_exist_in_source()
		{
			IList<MockResponse> source = new List<MockResponse>();

			IList<MockResponse> target = new List<MockResponse>
			{
				new MockResponse()
			};

			_synchronizer.Synchronize(source, target);

			Assert.AreEqual(0, target.Count);
		}
	}
}