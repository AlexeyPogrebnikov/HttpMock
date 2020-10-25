using System;
using System.Collections.Generic;
using System.Linq;

namespace TcpMock.Core
{
	public static class MockCache
	{
		private static IList<Mock> _mocks = new List<Mock>();
		private static readonly object SyncRoot = new object();

		public static void Add(Mock mock)
		{
			if (mock == null)
				throw new ArgumentNullException(nameof(mock));

			lock (SyncRoot)
			{
				_mocks.Add(mock);
			}
		}

		public static IEnumerable<Mock> GetAll()
		{
			lock (SyncRoot)
			{
				return _mocks.ToArray();
			}
		}

		public static void Init(Mock[] mocks)
		{
			lock (SyncRoot)
			{
				_mocks = new List<Mock>(mocks);
			}
		}
	}
}