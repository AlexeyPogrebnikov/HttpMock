﻿using System.Collections.Generic;
using HttpMock.Core;

namespace HttpMock.Client
{
	public class RouteCollectionSynchronizer
	{
		public void Synchronize(IList<Route> source, IList<Route> target)
		{
			AddToTarget(source, target);

			RemoveFromTarget(source, target);
		}

		private static void AddToTarget(IEnumerable<Route> source, ICollection<Route> target)
		{
			foreach (Route route in source)
			{
				if (!target.Contains(route))
					target.Add(route);
			}
		}

		private static void RemoveFromTarget(IList<Route> source, ICollection<Route> target)
		{
			var buffer = new List<Route>();

			foreach (Route route in target)
			{
				if (!source.Contains(route))
					buffer.Add(route);
			}

			foreach (Route route in buffer)
				target.Remove(route);
		}
	}
}