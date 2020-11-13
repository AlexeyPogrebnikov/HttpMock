using System.Net;

namespace HttpMock.Core
{
	public interface IHttpServer
	{
		bool IsStarted { get; }
		void Start(IPAddress address, int port);
		void Stop();
	}
}