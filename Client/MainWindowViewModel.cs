using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using HttpMock.Client.Commands;
using HttpMock.Core;

namespace HttpMock.Client
{
	public class MainWindowViewModel : INotifyPropertyChanged, IMainWindowViewModel
	{
		private Route _selectedRoute;
		private readonly IHttpServer _httpServer;
		private readonly IHttpInteractionCache _httpInteractionCache;
		private Action _refreshMocksListViewAction;

		public MainWindowViewModel()
		{
			_httpServer = ServiceLocator.Resolve<IHttpServer>();
			if (_httpServer != null)
			{
				IEnumerable<Route> mocks = _httpServer.Routes.GetAll();
				Routes = new ObservableCollection<Route>(mocks);
			}
			else
			{
				Routes = new ObservableCollection<Route>();
			}

			_httpInteractionCache = ServiceLocator.Resolve<IHttpInteractionCache>();

			HandledRequests = new ObservableCollection<HttpInteraction>();
			ClearHandledRequests = new ClearHandledRequestsCommand(this);

			UnhandledRequests = new ObservableCollection<HttpInteraction>();
			ClearUnhandledRequests = new ClearUnhandledRequestsCommand(this);

			ConnectionSettings = ConnectionSettingsCache.ConnectionSettings;

			SaveAs = new SaveAsCommand(_httpServer);
			Open = new OpenCommand(_httpServer, new MessageViewer());
			Exit = new ExitCommand();
			NewRoute = new NewRouteCommand();
			EditMock = new EditMockCommand(this);
			ClearMocks = new ClearMocksCommand(_httpServer);
			StartHttpServer = new StartHttpServerCommand(_httpServer, new MessageViewer());
			StopHttpServer = new StopHttpServerCommand(_httpServer);
			StartHttpServerVisibility = Visibility.Visible;
			StopHttpServerVisibility = Visibility.Hidden;
			AboutProgram = new AboutProgramCommand();

			RemoveMock = new RemoveMockCommand(_httpServer);
			RemoveMock.MockCollectionChanged += RemoveMock_MockCollectionChanged;

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

		private void RemoveMock_MockCollectionChanged(object sender, EventArgs e)
		{
			UpdateMocks();
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

			UpdateMocks();

			if (_httpInteractionCache != null)
			{
				IEnumerable<HttpInteraction> interactions = _httpInteractionCache.PopAll();

				foreach (HttpInteraction interaction in interactions)
				{
					if (interaction.Handled)
					{
						if (HandledRequests.All(rp => rp.Uid != interaction.Uid))
							HandledRequests.Insert(0, interaction);
					}
					else
					{
						if (UnhandledRequests.All(rp => rp.Uid != interaction.Uid))
							UnhandledRequests.Insert(0, interaction);
					}
				}
			}
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

		public EditMockCommand EditMock { get; }

		public ClearMocksCommand ClearMocks { get; }

		public ObservableCollection<Route> Routes { get; }

		public ObservableCollection<HttpInteraction> HandledRequests { get; }

		public ClearHandledRequestsCommand ClearHandledRequests { get; }

		public ObservableCollection<HttpInteraction> UnhandledRequests { get; }

		public void RefreshMocksListView()
		{
			_refreshMocksListViewAction();
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

		public RemoveMockCommand RemoveMock { get; }

		private void UpdateMocks()
		{
			if (_httpServer != null)
			{
				Route[] routes = _httpServer.Routes.GetAll().ToArray();

				RouteCollectionSynchronizer synchronizer = new();
				synchronizer.Synchronize(routes, Routes);
			}

			if (Routes.All(mock => mock.Uid != SelectedRoute?.Uid))
			{
				SelectedRoute = null;
			}
		}

		public void SetRefreshMocksListViewAction(Action action)
		{
			_refreshMocksListViewAction = action;
		}

		private void HttpServer_StatusChanged(object sender, EventArgs e)
		{
			OnPropertyChanged(nameof(IsEnabledEditConnectionSettings));
		}
	}
}