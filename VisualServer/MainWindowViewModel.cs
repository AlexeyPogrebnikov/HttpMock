﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using HttpMock.VisualServer.Commands;
using HttpMock.Core;

namespace HttpMock.VisualServer
{
	public class MainWindowViewModel : INotifyPropertyChanged, IMainWindowViewModel
	{
		private Route _selectedRoute;
		private readonly IHttpServer _httpServer;
		private Action _refreshRoutesListViewAction;

		public MainWindowViewModel()
		{
			_httpServer = ServiceLocator.Resolve<IHttpServer>();
			if (_httpServer != null)
			{
				IEnumerable<Route> routes = _httpServer.Routes.ToArray();
				Routes = new ObservableCollection<Route>(routes);
			}
			else
			{
				Routes = new ObservableCollection<Route>();
			}

			HandledRequests = new ObservableCollection<Interaction>();
			ClearHandledRequests = new ClearHandledRequestsCommand(this);

			UnhandledRequests = new ObservableCollection<Interaction>();
			ClearUnhandledRequests = new ClearUnhandledRequestsCommand(this);

			ConnectionSettings = ConnectionSettingsCache.ConnectionSettings;

			SaveAs = new SaveAsCommand(_httpServer);
			Open = new OpenCommand(_httpServer, new MessageViewer());
			Exit = new ExitCommand();
			NewRoute = new NewRouteCommand();
			EditRoute = new EditRouteCommand(this);
			ClearRoutes = new ClearRoutesCommand(_httpServer);
			StartHttpServer = new StartHttpServerCommand(_httpServer, new MessageViewer());
			StopHttpServer = new StopHttpServerCommand(_httpServer);
			StartHttpServerVisibility = Visibility.Visible;
			StopHttpServerVisibility = Visibility.Hidden;
			AboutProgram = new AboutProgramCommand();

			RemoveRoute = new RemoveRouteCommand(_httpServer);
			RemoveRoute.RouteCollectionChanged += RemoveRoute_RouteCollectionChanged;

			var dispatcherTimer = new DispatcherTimer
			{
				Interval = new TimeSpan(0, 0, 0, 0, 100)
			};

			dispatcherTimer.Tick += DispatcherTimer_Tick;
			dispatcherTimer.Start();
			_httpServer.StatusChanged += HttpServer_StatusChanged;
			Open.ServerProjectOpened += Open_ServerProjectOpened;
		}

		private void Open_ServerProjectOpened(object sender, EventArgs e)
		{
			OnPropertyChanged(nameof(ConnectionSettings));
		}

		private void RemoveRoute_RouteCollectionChanged(object sender, EventArgs e)
		{
			UpdateRoutes();
		}

		private void DispatcherTimer_Tick(object sender, EventArgs e)
		{
			if (_httpServer != null)
			{
				if (_httpServer.IsStarted)
				{
					StartHttpServerVisibility = Visibility.Collapsed;
					StopHttpServerVisibility = Visibility.Visible;
				}
				else
				{
					StartHttpServerVisibility = Visibility.Visible;
					StopHttpServerVisibility = Visibility.Collapsed;
				}
			}

			OnPropertyChanged(nameof(StartHttpServerVisibility));
			OnPropertyChanged(nameof(StopHttpServerVisibility));

			UpdateRoutes();
			UpdateRequests();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public SaveAsCommand SaveAs { get; }

		public OpenCommand Open { get; }

		public ExitCommand Exit { get; }

		public NewRouteCommand NewRoute { get; }

		public EditRouteCommand EditRoute { get; }

		public ClearRoutesCommand ClearRoutes { get; }

		public ObservableCollection<Route> Routes { get; }

		public ObservableCollection<Interaction> HandledRequests { get; }

		public ClearHandledRequestsCommand ClearHandledRequests { get; }

		public ObservableCollection<Interaction> UnhandledRequests { get; }

		public void RefreshRouteListView()
		{
			_refreshRoutesListViewAction();
		}

		public ClearUnhandledRequestsCommand ClearUnhandledRequests { get; }

		public ConnectionSettings ConnectionSettings { get; }

		public bool IsEnabledEditConnectionSettings => !_httpServer.IsStarted;

		public StartHttpServerCommand StartHttpServer { get; }

		public Visibility StartHttpServerVisibility { get; set; }

		public StopHttpServerCommand StopHttpServer { get; }

		public Visibility StopHttpServerVisibility { get; set; }

		public AboutProgramCommand AboutProgram { get; }

		public Route SelectedRoute
		{
			get => _selectedRoute;
			set
			{
				_selectedRoute = value;
				OnPropertyChanged(nameof(SelectedRoute));
			}
		}

		public RemoveRouteCommand RemoveRoute { get; }

		private void UpdateRoutes()
		{
			if (_httpServer != null)
			{
				Route[] routes = _httpServer.Routes.ToArray();

				RouteCollectionSynchronizer synchronizer = new();
				synchronizer.Synchronize(routes, Routes);
			}

			if (Routes.Contains(SelectedRoute))
			{
				SelectedRoute = null;
			}
		}

		private void UpdateRequests()
		{
			if (_httpServer != null)
			{
				IEnumerable<Interaction> interactions = _httpServer.Interactions.PopAll();

				foreach (Interaction interaction in interactions)
				{
					if (interaction.Request.Handled)
						HandledRequests.Insert(0, interaction);
					else
						UnhandledRequests.Insert(0, interaction);
				}
			}
		}

		public void SetRefreshRoutesListViewAction(Action action)
		{
			_refreshRoutesListViewAction = action;
		}

		private void HttpServer_StatusChanged(object sender, EventArgs e)
		{
			OnPropertyChanged(nameof(IsEnabledEditConnectionSettings));
		}
	}
}