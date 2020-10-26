using System.Collections.Generic;

namespace TcpMock.Core
{
	public interface ITcpInteractionCache
	{
		void Add(TcpInteraction tcpInteraction);
		IEnumerable<TcpInteraction> GetAll();
	}
}