using System.Collections.Generic;
using System.Linq;

namespace HttpMock.Core
{
	public class TcpInteractionCache : ITcpInteractionCache
	{
		private readonly IList<TcpInteraction> _tcpInteractions = new List<TcpInteraction>();
		private readonly object _syncRoot = new object();

		public void Add(TcpInteraction tcpInteraction)
		{
			lock (_syncRoot)
			{
				_tcpInteractions.Add(tcpInteraction);
			}
		}

		public IEnumerable<TcpInteraction> GetAll()
		{
			lock (_syncRoot)
			{
				return _tcpInteractions.ToArray();
			}
		}
	}
}