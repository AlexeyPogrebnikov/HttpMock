using System;
using System.Net;
using HttpMock.Core;

namespace HttpMock.VisualServer
{
	public interface IVisualHttpServer
	{
		event EventHandler StatusChanged;
		
		bool IsStarted { get; }
		RouteCollection Routes { get; }
		InteractionCollection Interactions { get; }

		void Start(IPAddress address, int port);
		void Stop();
	}
}