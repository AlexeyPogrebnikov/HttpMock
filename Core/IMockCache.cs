using System.Collections.Generic;

namespace HttpMock.Core
{
	public interface IMockCache
	{
		void Init(Route[] mocks);
		void Add(Route mock);
		void Remove(Route mock);
		void Clear();
		IEnumerable<Route> GetAll();
		bool Contains(Route mock);
	}
}