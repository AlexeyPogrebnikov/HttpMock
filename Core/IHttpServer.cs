﻿using System;
using System.Net;

namespace HttpMock.Core
{
	public interface IHttpServer
	{
		event EventHandler StatusChanged;
		
		bool IsStarted { get; }
		RouteCollection Routes { get; }

		void Start(IPAddress address, int port);
		void Stop();
	}
}