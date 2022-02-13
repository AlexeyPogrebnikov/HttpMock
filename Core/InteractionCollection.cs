using System;
using System.Collections.Generic;

namespace HttpMock.Core
{
	public class InteractionCollection
	{
		private IList<Interaction> _interactions = new List<Interaction>();
		private readonly object _syncRoot = new();

		public event EventHandler ItemAdded;

		public void Add(Interaction httpInteraction)
		{
			lock (_syncRoot)
			{
				_interactions.Add(httpInteraction);
			}

			ItemAdded?.Invoke(this, EventArgs.Empty);
		}

		public IEnumerable<Interaction> PopAll()
		{
			lock (_syncRoot)
			{
				var interactions = _interactions;
				_interactions = new List<Interaction>();
				return interactions;
			}
		}
	}
}