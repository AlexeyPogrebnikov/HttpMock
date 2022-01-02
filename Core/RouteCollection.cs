using System;
using System.Collections.Generic;
using System.Linq;

namespace HttpMock.Core
{
	public class RouteCollection
	{
		private List<Route> _routes = new();
		private readonly object _syncRoot = new();

		public void Init(Route[] routes)
		{
			lock (_syncRoot)
			{
				foreach (Route route in routes)
					CheckRoute(route);

				_routes = new(routes);
			}
		}

		public void Add(Route route)
		{
			lock (_syncRoot)
			{
				CheckRoute(route);
				_routes.Add(route);
			}
		}

		public void Remove(Route route)
		{
			lock (_syncRoot)
			{
				_routes.Remove(route);
			}
		}

		public void Clear()
		{
			lock (_syncRoot)
			{
				_routes.Clear();
			}
		}

		public Route[] ToArray()
		{
			lock (_syncRoot)
			{
				return _routes.ToArray();
			}
		}

		public IEnumerable<Route> Find(string method, string path)
		{
			lock (_syncRoot)
			{
				return DoFind(method, path).ToArray();
			}
		}

		public bool Contains(Route route)
		{
			lock (_syncRoot)
			{
				return DoFind(route.Method, route.Path).Any();
			}
		}

		private IEnumerable<Route> DoFind(string method, string path)
		{
			return _routes.Where(m => m.Method == method && m.Path == path);
		}

		private static void CheckRoute(Route route)
		{
			if (route == null)
				throw new ArgumentNullException(nameof(route));

			if (string.IsNullOrEmpty(route.Method))
				throw new ArgumentException("Method of the route is null or empty.");

			if (string.IsNullOrEmpty(route.Path))
				throw new ArgumentException("Path of the route is null or empty.");

			if (string.IsNullOrEmpty(route.Response.StatusCode))
				throw new ArgumentException("StatusCode of the response is null or empty.");
		}
	}
}