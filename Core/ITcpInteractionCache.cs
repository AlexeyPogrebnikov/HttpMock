using System.Collections.Generic;

namespace HttpMock.Core
{
	public interface ITcpInteractionCache
	{
		void Add(TcpInteraction tcpInteraction);
		IEnumerable<TcpInteraction> GetAll();
	}
}