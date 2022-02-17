using System;
using System.Net;
using System.Threading.Tasks;
using HttpMock.Core;

namespace HttpMock.VisualServer
{
	public interface IVisualHttpServer
	{
		bool IsStarted { get; }
		bool StartEnabled { get; }
		bool StopEnabled { get; }
		RouteCollection Routes { get; }
		InteractionCollection HandledInteractions { get; }
		InteractionCollection UnhandledInteractions { get; }
		event EventHandler StatusChanged;

		/// <exception cref="InvalidOperationException"></exception>
		Task StartAsync(IPAddress address, int port);

		/// <exception cref="InvalidOperationException"></exception>
		Task StopAsync();
	}
}