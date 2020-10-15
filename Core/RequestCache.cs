using System.Collections.Generic;
using System.Linq;

namespace TcpMock.Core
{
	public static class RequestCache
	{
		private static readonly IList<Request> Requests = new List<Request>();
		private static readonly object SyncRoot = new object();

		public static void Add(Request request)
		{
			lock (SyncRoot)
			{
				Requests.Add(request);
			}
		}

		public static IEnumerable<Request> GetAll()
		{
			lock (SyncRoot)
			{
				return Requests.ToArray();
			}
		}
	}
}