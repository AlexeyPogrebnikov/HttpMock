using System;
using System.Collections.Generic;
using HttpMock.Client;
using HttpMock.Core;
using NUnit.Framework;

namespace HttpMock.VisualServer.Tests
{
	[TestFixture]
	public class RouteCollectionSynchronizerTests
	{
		private RouteCollectionSynchronizer _synchronizer;

		[SetUp]
		public void SetUp()
		{
			_synchronizer = new RouteCollectionSynchronizer();
		}

		[Test]
		public void Synchronize_add_route_to_target()
		{
			var route = new Route
			{
				Method = "GET",
				Path = "/"
			};

			IList<Route> source = new[]
			{
				route
			};

			IList<Route> target = new List<Route>();

			_synchronizer.Synchronize(source, target);

			Assert.AreEqual(1, target.Count);
			Assert.AreSame(route, target[0]);
		}

		[Test]
		public void Synchronize_dont_add_existing_route_to_target()
		{
			var uid = new Guid("261B98BB-6BC7-4809-96E4-194A14EE36C6");

			IList<Route> source = new List<Route>
			{
				new Route
				{
					Method = "POST",
					Path = "/item"
				}
			};

			IList<Route> target = new List<Route>
			{
				new Route
				{
					Method = "POST",
					Path = "/item"
				}
			};

			_synchronizer.Synchronize(source, target);

			Assert.AreEqual(1, target.Count);
		}

		[Test]
		public void Synchronize_remove_route_from_target_if_its_does_not_exist_in_source()
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