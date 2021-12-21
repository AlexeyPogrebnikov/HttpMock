using System.Collections.Generic;
using System.Linq;
using HttpMock.Core;

namespace HttpMock.Client
{
	public class MockCollectionSynchronizer
	{
		public void Synchronize(IList<MockResponse> source, IList<MockResponse> target)
		{
			AddToTarget(source, target);

			RemoveFromTarget(source, target);
		}

		private static void AddToTarget(IEnumerable<MockResponse> source, ICollection<MockResponse> target)
		{
			foreach (MockResponse mock in source)
			{
				if (target.All(m => m.Uid != mock.Uid))
					target.Add(mock);
			}
		}

		private static void RemoveFromTarget(IList<MockResponse> source, ICollection<MockResponse> target)
		{
			var buffer = new List<MockResponse>();

			foreach (MockResponse mock in target)
			{
				if (source.All(m => m.Uid != mock.Uid))
					buffer.Add(mock);
			}

			foreach (MockResponse mock in buffer)
				target.Remove(mock);
		}
	}
}