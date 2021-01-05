using System.Collections.Generic;

namespace HttpMock.Core
{
	public interface IHttpInteractionCache
	{
		void Add(HttpInteraction httpInteraction);
		IEnumerable<HttpInteraction> PopAll();
	}
}