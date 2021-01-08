using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HttpMock.Core
{
	public class MockCache : IMockCache
	{
		private List<Mock> _mocks = new List<Mock>();

		public void Add(Mock mock)
		{
			CheckMock(mock);

			List<Mock> newMocks = _mocks.ToList();
			newMocks.Add(mock);
			Interlocked.Exchange(ref _mocks, newMocks);
		}

		public void AddRange(Mock[] mocks)
		{
			foreach (Mock mock in mocks)
				CheckMock(mock);

			List<Mock> newMocks = _mocks.ToList();
			newMocks.AddRange(mocks);
			Interlocked.Exchange(ref _mocks, newMocks);
		}

		public void Remove(Mock mock)
		{
			List<Mock> newMocks = _mocks.ToList();
			newMocks.Remove(mock);
			Interlocked.Exchange(ref _mocks, newMocks);
		}

		public void Clear()
		{
			Interlocked.Exchange(ref _mocks, new List<Mock>());
		}

		public IEnumerable<Mock> GetAll()
		{
			return _mocks;
		}

		public bool Contains(Mock mock)
		{
			return _mocks.Any(m => m.Method == mock.Method && m.Path == mock.Path);
		}

		private void CheckMock(Mock mock)
		{
			if (mock == null)
				throw new ArgumentNullException(nameof(mock));

			if (string.IsNullOrEmpty(mock.Method))
				throw new ArgumentException("Method of the mock is null or empty.");

			if (string.IsNullOrEmpty(mock.StatusCode))
				throw new ArgumentException("StatusCode of the mock is null or empty.");

			if (Contains(mock))
				throw new InvalidOperationException("Mock with same Method and Path already exists.");
		}
	}
}