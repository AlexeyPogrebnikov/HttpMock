using System.Collections.Generic;

namespace HttpMock.Core
{
	public interface IMockCache
	{
		void Add(Mock mock);
		IEnumerable<Mock> GetAll();
		void Init(Mock[] mocks);
		void Remove(Mock mock);
		void Clear();
	}
}