using System.Collections.Generic;
using System.Linq;
using HttpMock.Core;

namespace HttpMock.Client
{
	public class MockCollectionSynchronizer
	{
		public void Synchronize(IList<Route> source, IList<Route> target)
		{
			AddToTarget(source, target);

			RemoveFromTarget(source, target);
		}

		private static void AddToTarget(IEnumerable<Route> source, ICollection<Route> target)
		{
			foreach (Route mock in source)
			{
				if (target.All(m => m.Uid != mock.Uid))
					target.Add(mock);
			}
		}

		private static void RemoveFromTarget(IList<Route> source, ICollection<Route> target)
		{
			var buffer = new List<Route>();

			foreach (Route mock in target)
			{
				if (source.All(m => m.Uid != mock.Uid))
					buffer.Add(mock);
			}

			foreach (Route mock in buffer)
				target.Remove(mock);
		}
	}
}