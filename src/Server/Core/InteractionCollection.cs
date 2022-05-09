using System;
using System.Collections.Generic;

namespace HttpMock.Server.Core
{
	public class InteractionCollection
	{
		private readonly object _syncRoot = new();
		private IList<Interaction> _interactions = new List<Interaction>();

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
				IList<Interaction> interactions = _interactions;
				_interactions = new List<Interaction>();
				return interactions;
			}
		}
	}
}