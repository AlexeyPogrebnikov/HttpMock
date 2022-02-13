using System;
using HttpMock.Core;
using HttpMock.VisualServer.Model;
using Microsoft.Extensions.DependencyInjection;

namespace HttpMock.VisualServer
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
			services.AddSingleton<IVisualHttpServer, VisualHttpServer>();
			services.AddSingleton<RouteUICollection, RouteUICollection>();
			services.AddSingleton<IMessageViewer, MessageViewer>();
		}
	}
}