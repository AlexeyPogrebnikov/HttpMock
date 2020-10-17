using System.Collections.Generic;
using System.Linq;

namespace TcpMock.Core
{
	public static class TcpInteractionCache
	{
		private static readonly IList<TcpInteraction> TcpInteractions = new List<TcpInteraction>();
		private static readonly object SyncRoot = new object();

		public static void Add(TcpInteraction tcpInteraction)
		{
			lock (SyncRoot)
			{
				TcpInteractions.Add(tcpInteraction);
			}
		}

		public static IEnumerable<TcpInteraction> GetAll()
		{
			lock (SyncRoot)
			{
				return TcpInteractions.ToArray();
			}
		}
	}
}