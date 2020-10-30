﻿using System;
using Microsoft.Extensions.DependencyInjection;
using TcpMock.Core;

namespace TcpMock.Client
{
	public static class ServiceLocator
	{
		private static ServiceProvider _serviceProvider;
		private static bool _isInitialized;

		public static void Init()
		{
			if (_isInitialized)
				throw new InvalidOperationException("ServiceLocator already initialized.");

			_isInitialized = true;

			var serviceCollection = new ServiceCollection();

			ConfigureServices(serviceCollection);

			_serviceProvider = serviceCollection.BuildServiceProvider();
		}

		public static T Resolve<T>()
		{
			if (!_isInitialized)
				return default;

			return (T) _serviceProvider.GetService(typeof(T));
		}

		private static void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IMockCache, MockCache>();
			services.AddSingleton<ITcpServer, TcpServer>();
			services.AddSingleton<ITcpInteractionCache, TcpInteractionCache>();
		}
	}
}