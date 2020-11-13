using System.Collections.Generic;
using System.Linq;
using HttpMock.Core;

namespace HttpMock.Client
{
	public class MockCollectionSynchronizer
	{
		public void Synchronize(IList<Mock> source, IList<Mock> target)
		{
			AddToTarget(source, target);

			RemoveFromTarget(source, target);
		}

		private static void AddToTarget(IEnumerable<Mock> source, ICollection<Mock> target)
		{
			foreach (Mock mock in source)
			{
				if (target.All(m => m.Uid != mock.Uid))
					target.Add(mock);
			}
		}

		private static void RemoveFromTarget(IList<Mock> source, ICollection<Mock> target)
		{
			var buffer = new List<Mock>();

			foreach (Mock mock in target)
			{
				if (source.All(m => m.Uid != mock.Uid))
					buffer.Add(mock);
			}

			foreach (Mock mock in buffer)
				target.Remove(mock);
		}
	}
}