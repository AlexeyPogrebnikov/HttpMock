using System.Collections.Generic;

namespace HttpMock.Core
{
	public interface IMockCache
	{
		void Init(MockResponse[] mocks);
		void Add(MockResponse mock);
		void Remove(MockResponse mock);
		void Clear();
		IEnumerable<MockResponse> GetAll();
		bool Contains(MockResponse mock);
	}
}