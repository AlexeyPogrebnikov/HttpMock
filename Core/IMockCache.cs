using System.Collections.Generic;

namespace TcpMock.Core
{
	public interface IMockCache
	{
		void Add(Mock mock);
		IEnumerable<Mock> GetAll();
		void Init(Mock[] mocks);
	}
}