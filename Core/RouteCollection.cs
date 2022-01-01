using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HttpMock.Core
{
	public class RouteCollection
	{
		private List<Route> _routes = new();

		public void Init(Route[] routes)
		{
			foreach (Route route in routes)
				CheckRoute(route);

			List<Route> newRoutes = routes.ToList();
			Interlocked.Exchange(ref _routes, newRoutes);
		}

		public void Add(Route route)
		{
			CheckRoute(route);

			List<Route> newRoutes = _routes.ToList();
			newRoutes.Add(route);
			Interlocked.Exchange(ref _routes, newRoutes);
		}

		public void Remove(Route route)
		{
			List<Route> newRoutes = _routes.ToList();
			newRoutes.Remove(route);
			Interlocked.Exchange(ref _routes, newRoutes);
		}

		public void Clear()
		{
			Interlocked.Exchange(ref _routes, new List<Route>());
		}

		public IEnumerable<Route> GetAll()
		{
			return _routes;
		}

		public bool Contains(Route route)
		{
			return _routes.Any(m => m.Method == route.Method && m.Path == route.Path);
		}

		private void CheckRoute(Route route)
		{
			if (route == null)
				throw new ArgumentNullException(nameof(route));

			if (string.IsNullOrEmpty(route.Method))
				throw new ArgumentException("Method of the route is null or empty.");

			if (string.IsNullOrEmpty(route.Path))
				throw new ArgumentException("Path of the route is null or empty.");

			if (string.IsNullOrEmpty(route.Response.StatusCode))
				throw new ArgumentException("StatusCode of the response is null or empty.");

			if (Contains(route))
				throw new InvalidOperationException("Route with same Method and Path already exists.");
		}
	}
}