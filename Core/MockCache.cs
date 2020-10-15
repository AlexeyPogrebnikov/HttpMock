using System.Collections.Generic;
using System.Linq;

namespace TcpMock.Core
{
	public static class MockCache
	{
		private static readonly IList<Mock> Mocks = new List<Mock>();
		private static readonly object SyncRoot = new object();

		public static void Add(Mock mock)
		{
			lock (SyncRoot)
			{
				Mocks.Add(mock);
			}
		}

		public static IEnumerable<Mock> GetAll()
		{
			lock (SyncRoot)
			{
				return Mocks.ToArray();
			}
		}
	}
}