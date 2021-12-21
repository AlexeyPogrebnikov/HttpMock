using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HttpMock.Core
{
	public class MockCache : IMockCache
	{
		private List<MockResponse> _mocks = new();

		public void Init(MockResponse[] mocks)
		{
			foreach (MockResponse mock in mocks)
				CheckMock(mock);

			List<MockResponse> newMocks = mocks.ToList();
			Interlocked.Exchange(ref _mocks, newMocks);
		}

		public void Add(MockResponse mock)
		{
			CheckMock(mock);

			List<MockResponse> newMocks = _mocks.ToList();
			newMocks.Add(mock);
			Interlocked.Exchange(ref _mocks, newMocks);
		}

		public void Remove(MockResponse mock)
		{
			List<MockResponse> newMocks = _mocks.ToList();
			newMocks.Remove(mock);
			Interlocked.Exchange(ref _mocks, newMocks);
		}

		public void Clear()
		{
			Interlocked.Exchange(ref _mocks, new List<MockResponse>());
		}

		public IEnumerable<MockResponse> GetAll()
		{
			return _mocks;
		}

		public bool Contains(MockResponse mock)
		{
			return _mocks.Any(m => m.Method == mock.Method && m.Path == mock.Path);
		}

		private void CheckMock(MockResponse mock)
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