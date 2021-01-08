using System.Collections.Generic;

namespace HttpMock.Core
{
	public interface IMockCache
	{
		void Add(Mock mock);
		void AddRange(Mock[] mocks);
		void Remove(Mock mock);
		void Clear();
		IEnumerable<Mock> GetAll();
		bool Contains(Mock mock);
	}
}