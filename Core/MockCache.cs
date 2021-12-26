using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HttpMock.Core
{
	public class MockCache : IMockCache
	{
		private List<Route> _mocks = new();

		public void Init(Route[] mocks)
		{
			foreach (Route mock in mocks)
				CheckMock(mock);

			List<Route> newMocks = mocks.ToList();
			Interlocked.Exchange(ref _mocks, newMocks);
		}

		public void Add(Route mock)
		{
			CheckMock(mock);

			List<Route> newMocks = _mocks.ToList();
			newMocks.Add(mock);
			Interlocked.Exchange(ref _mocks, newMocks);
		}

		public void Remove(Route mock)
		{
			List<Route> newMocks = _mocks.ToList();
			newMocks.Remove(mock);
			Interlocked.Exchange(ref _mocks, newMocks);
		}

		public void Clear()
		{
			Interlocked.Exchange(ref _mocks, new List<Route>());
		}

		public IEnumerable<Route> GetAll()
		{
			return _mocks;
		}

		public bool Contains(Route mock)
		{
			return _mocks.Any(m => m.Method == mock.Method && m.Path == mock.Path);
		}

		private void CheckMock(Route mock)
		{
			if (mock == null)
				throw new ArgumentNullException(nameof(mock));

			if (string.IsNullOrEmpty(mock.Method))
				throw new ArgumentException("Method of the route is null or empty.");

			if (string.IsNullOrEmpty(mock.Path))
				throw new ArgumentException("Path of the route is null or empty.");

			if (string.IsNullOrEmpty(mock.Response.StatusCode))
				throw new ArgumentException("StatusCode of the response is null or empty.");

			if (Contains(mock))
				throw new InvalidOperationException("Route with same Method and Path already exists.");
		}
	}
}