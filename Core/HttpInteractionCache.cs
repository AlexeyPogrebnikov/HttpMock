using System.Collections.Generic;
using System.Linq;

namespace HttpMock.Core
{
	public class HttpInteractionCache : IHttpInteractionCache
	{
		private readonly IList<HttpInteraction> _httpInteractions = new List<HttpInteraction>();
		private readonly object _syncRoot = new object();

		public void Add(HttpInteraction httpInteraction)
		{
			lock (_syncRoot)
			{
				_httpInteractions.Add(httpInteraction);
			}
		}

		public IEnumerable<HttpInteraction> PopAll()
		{
			lock (_syncRoot)
			{
				HttpInteraction[] interactions = _httpInteractions.ToArray();
				_httpInteractions.Clear();
				return interactions;
			}
		}
	}
}