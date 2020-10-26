using System;
using System.Collections.Generic;
using System.Linq;

namespace TcpMock.Core
{
	public class MockCache : IMockCache
	{
		private IList<Mock> _mocks = new List<Mock>();
		private readonly object SyncRoot = new object();

		public void Add(Mock mock)
		{
			if (mock == null)
				throw new ArgumentNullException(nameof(mock));

			lock (SyncRoot)
			{
				_mocks.Add(mock);
			}
		}

		public IEnumerable<Mock> GetAll()
		{
			lock (SyncRoot)
			{
				return _mocks.ToArray();
			}
		}

		public void Init(Mock[] mocks)
		{
			lock (SyncRoot)
			{
				_mocks = new List<Mock>(mocks);
			}
		}
	}
}