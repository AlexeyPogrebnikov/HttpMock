﻿using System;
using System.Linq;
using HttpMock.Server.Core;
using NUnit.Framework;

namespace HttpMock.Server.Tests.Core
{
	[TestFixture]
	public class RouteCollectionTests
	{
		[Test]
		public void Init_throw_ArgumentException_if_Method_is_null()
		{
			var routes = new RouteCollection();
			var route = new Route {Method = null};

			var exception = Assert.Throws<ArgumentException>(() => routes.Init(new[] {route}));
			Assert.AreEqual("Method of the route is null or empty.", exception.Message);
		}

		[Test]
		public void Init_throw_ArgumentException_if_StatusCode_is_empty()
		{
			var routes = new RouteCollection();
			var route = new Route
			{
				Method = "GET",
				Path = "/",
				Response = new Response
				{
					StatusCode = 0
				}
			};

			var exception = Assert.Throws<ArgumentException>(() => routes.Init(new[] {route}));
			Assert.AreEqual("StatusCode of the response is 0.", exception.Message);
		}

		[Test]
		public void Init_clear_previous_routes()
		{
			RouteCollection routeCollection = new();

			routeCollection.Init(new[]
			{
				new Route
				{
					Method = "GET",
					Path = "/",
					Response = new Response
					{
						StatusCode = 200
					}
				}
			});

			Route[] routes =
			{
				new()
				{
					Method = "GET",
					Path = "/clients",
					Response = new Response
					{
						StatusCode = 200
					}
				},
				new()
				{
					Method = "GET",
					Path = "/orders",
					Response = new Response
					{
						StatusCode = 200
					}
				}
			};

			routeCollection.Init(routes);

			Assert.AreEqual(2, routeCollection.ToArray().Length);
		}

		[Test]
		public void Init_throw_ArgumentNullException_if_route_is_null()
		{
			var routeCollection = new RouteCollection();
			var routes = new Route[] {null};
			Assert.Throws<ArgumentNullException>(() => routeCollection.Init(routes));
		}

		[Test]
		public void GetAll_return_all_added_routes()
		{
			var routeCollection = new RouteCollection();
			var route = new Route
			{
				Method = "GET",
				Path = "/",
				Response = new Response
				{
					StatusCode = 200
				}
			};
			routeCollection.Init(new[] {route});

			Route[] routes = routeCollection.ToArray();

			Assert.AreEqual(1, routes.Length);
			Assert.AreSame(route, routes[0]);
		}

		[Test]
		public void Init_not_throw_exception_if_Paths_are_not_same()
		{
			var routeCollection = new RouteCollection();
			var firstRoute = new Route
			{
				Method = "GET",
				Path = "/",
				Response = new Response
				{
					StatusCode = 200
				}
			};

			var secondRoute = new Route
			{
				Method = "GET",
				Path = "/s",
				Response = new Response
				{
					StatusCode = 500
				}
			};

			routeCollection.Init(new[] {firstRoute, secondRoute});
		}
	}
}