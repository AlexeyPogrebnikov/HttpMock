using System;
using System.Collections.Generic;
using System.Linq;

namespace TcpMock.Core
{
	public class MockCache : IMockCache
	{
		private IList<Mock> _mocks = new List<Mock>();
		private readonly object _syncRoot = new object();

		public void Add(Mock mock)
		{
			if (mock == null)
				throw new ArgumentNullException(nameof(mock));

			lock (_syncRoot)
			{
				_mocks.Add(mock);
			}
		}

		public IEnumerable<Mock> GetAll()
		{
			lock (_syncRoot)
			{
				return _mocks.ToArray();
			}
		}

		public void Init(Mock[] mocks)
		{
			lock (_syncRoot)
			{
				_mocks = new List<Mock>(mocks);
			}
		}

		public void Remove(Mock mock)
		{
			lock (_syncRoot)
			{
				_mocks.Remove(mock);
			}
		}
	}
}