using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using HttpMock.VisualServer.Commands;
using HttpMock.Core;
using HttpMock.VisualServer.Model;

namespace HttpMock.VisualServer
{
	public class MainWindowViewModel : INotifyPropertyChanged, IMainWindowViewModel
	{
		private Route _selectedRoute;
		private readonly IVisualHttpServer _httpServer;
		private Action _refreshRoutesListViewAction;

		public MainWindowViewModel()
		{
			_httpServer = ServiceLocator.Resolve<IVisualHttpServer>();
			var routes = ServiceLocator.Resolve<RouteUICollection>();
			if (routes != null)
				Routes = routes.AsObservable();

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
			ClearRoutes = new ClearRoutesCommand(routes);
			StartHttpServer = new StartHttpServerCommand(_httpServer, new MessageViewer());
			StopHttpServer = new StopHttpServerCommand(_httpServer, new MessageViewer());
			StartHttpServerVisibility = Visibility.Visible;
			StopHttpServerVisibility = Visibility.Hidden;
			AboutProgram = new AboutProgramCommand();

			RemoveRoute = new RemoveRouteCommand(routes);

			var dispatcherTimer = new DispatcherTimer
			{
				Interval = new TimeSpan(0, 0, 0, 0, 100)
			};

			dispatcherTimer.Tick += DispatcherTimer_Tick;
			dispatcherTimer.Start();
			if (_httpServer != null)
				_httpServer.StatusChanged += HttpServer_StatusChanged;
			Open.ServerProjectOpened += Open_ServerProjectOpened;
		}

		private void Open_ServerProjectOpened(object sender, EventArgs e)
		{
			OnPropertyChanged(nameof(ConnectionSettings));
		}

		private void DispatcherTimer_Tick(object sender, EventArgs e)
		{
			if (_httpServer != null)
			{
				if (_httpServer.StartEnabled && !_httpServer.StopEnabled)
				{
					StartHttpServerVisibility = Visibility.Visible;
					StopHttpServerVisibility = Visibility.Collapsed;
				}
				else if (!_httpServer.StartEnabled && _httpServer.StopEnabled)
				{
					StartHttpServerVisibility = Visibility.Collapsed;
					StopHttpServerVisibility = Visibility.Visible;
				}
				else
				{
					StartHttpServerVisibility = Visibility.Collapsed;
					StopHttpServerVisibility = Visibility.Collapsed;
				}

				if (!_httpServer.StartEnabled && !_httpServer.StopEnabled)
					StoppingHttpServerVisibility = Visibility.Visible;
				else
					StoppingHttpServerVisibility = Visibility.Collapsed;
			}

			OnPropertyChanged(nameof(StartHttpServerVisibility));
			OnPropertyChanged(nameof(StopHttpServerVisibility));
			OnPropertyChanged(nameof(StoppingHttpServerVisibility));

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

		public ObservableCollection<RouteUI> Routes { get; }

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

		public Visibility StoppingHttpServerVisibility { get; set; }

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