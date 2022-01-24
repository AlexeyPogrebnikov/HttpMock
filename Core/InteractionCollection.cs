using System;
using System.Collections.Generic;
using System.Linq;

namespace HttpMock.Core
{
	public class InteractionCollection
	{
		private readonly IList<Interaction> _httpInteractions = new List<Interaction>();
		private readonly object _syncRoot = new();

		public event EventHandler ItemAdded;

		public void Add(Interaction httpInteraction)
		{
			lock (_syncRoot)
			{
				_httpInteractions.Add(httpInteraction);
			}

			ItemAdded?.Invoke(this, EventArgs.Empty);
		}

		public IEnumerable<Interaction> PopAll()
		{
			lock (_syncRoot)
			{
				Interaction[] interactions = _httpInteractions.ToArray();
				_httpInteractions.Clear();
				return interactions;
			}
		}
	}
}