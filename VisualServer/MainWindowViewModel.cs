using System;
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

			SaveAs = ServiceLocator.Resolve<SaveAsCommand>();
			Open = ServiceLocator.Resolve<OpenCommand>();
			Exit = new ExitCommand();
			NewRoute = new NewRouteCommand();
			EditRoute = new EditRouteCommand(this);
			ClearRoutes = ServiceLocator.Resolve<ClearRoutesCommand>();
			StartHttpServer = ServiceLocator.Resolve<StartHttpServerCommand>();
			StopHttpServer = ServiceLocator.Resolve<StopHttpServerCommand>();
			StartHttpServerVisibility = Visibility.Visible;
			StopHttpServerVisibility = Visibility.Hidden;
			AboutProgram = new AboutProgramCommand();

			RemoveRoute = ServiceLocator.Resolve<RemoveRouteCommand>();

			var dispatcherTimer = new DispatcherTimer
			{
				Interval = new TimeSpan(0, 0, 0, 0, 100)
			};

			dispatcherTimer.Tick += DispatcherTimer_Tick;
			dispatcherTimer.Start();
			if (_httpServer != null)
				_httpServer.StatusChanged += HttpServer_StatusChanged;
			if (Open != null)
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

		public bool IsEnabledEditConnectionSettings => _httpServer.StartEnabled;

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
				foreach (Interaction interaction in _httpServer.HandledInteractions.PopAll())
					HandledRequests.Insert(0, interaction);

				foreach (Interaction interaction in _httpServer.UnhandledInteractions.PopAll())
					UnhandledRequests.Insert(0, interaction);
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