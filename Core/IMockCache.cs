using System.Collections.Generic;

namespace HttpMock.Core
{
	public interface IMockCache
	{
		void Init(Mock[] mocks);
		void Add(Mock mock);
		void Remove(Mock mock);
		void Clear();
		IEnumerable<Mock> GetAll();
		bool Contains(Mock mock);
	}
}