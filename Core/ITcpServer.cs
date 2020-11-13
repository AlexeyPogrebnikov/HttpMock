﻿using System.Net;

namespace HttpMock.Core
{
	public interface ITcpServer
	{
		bool IsStarted { get; }
		void Start(IPAddress address, int port);
		void Stop();
	}
}