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
			var mock = new Route
			{
				Uid = new Guid("20037054-3CC0-4457-9686-2B8A8C8B5814")
			};

			IList<Route> source = new[]
			{
				mock
			};

			IList<Route> target = new List<Route>();

			_synchronizer.Synchronize(source, target);

			Assert.AreEqual(1, target.Count);
			Assert.AreSame(mock, target[0]);
		}

		[Test]
		public void Synchronize_dont_add_existing_mock_to_target()
		{
			var uid = new Guid("261B98BB-6BC7-4809-96E4-194A14EE36C6");

			IList<Route> source = new List<Route>
			{
				new Route
				{
					Uid = uid
				}
			};

			IList<Route> target = new List<Route>
			{
				new Route
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
			IList<Route> source = new List<Route>();

			IList<Route> target = new List<Route>
			{
				new Route()
			};

			_synchronizer.Synchronize(source, target);

			Assert.AreEqual(0, target.Count);
		}
	}
}