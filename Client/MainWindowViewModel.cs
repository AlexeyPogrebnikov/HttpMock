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
		private Mock _selectedMock;
		private readonly IHttpServer _httpServer;
		private readonly IHttpInteractionCache _httpInteractionCache;
		private readonly IMockCache _mockCache;

		public MainWindowViewModel()
		{
			_httpServer = ServiceLocator.Resolve<IHttpServer>();
			_mockCache = ServiceLocator.Resolve<IMockCache>();
			if (_mockCache != null)
			{
				IEnumerable<Mock> mocks = _mockCache.GetAll();
				Mocks = new ObservableCollection<Mock>(mocks);
			}
			else
			{
				Mocks = new ObservableCollection<Mock>();
			}

			_httpInteractionCache = ServiceLocator.Resolve<IHttpInteractionCache>();

			HandledRequests = new ObservableCollection<HttpInteraction>();
			ClearHandledRequests = new ClearHandledRequestsCommand(this);

			UnhandledRequests = new ObservableCollection<HttpInteraction>();
			ClearUnhandledRequests = new ClearUnhandledRequestsCommand(this);

			ConnectionSettings = ConnectionSettingsCache.ConnectionSettings;

			Exit = new ExitCommand();
			NewMock = new NewMockCommand();
			ClearMocks = new ClearMocksCommand(_mockCache);
			StartHttpServer = new StartHttpServerCommand(_httpServer);
			StopHttpServer = new StopHttpServerCommand(_httpServer);
			StartHttpServerVisibility = Visibility.Visible;
			StopHttpServerVisibility = Visibility.Hidden;
			AboutProgram = new AboutProgramCommand();

			RemoveMock = new RemoveMockCommand(_mockCache);
			RemoveMock.MockCollectionChanged += RemoveMock_MockCollectionChanged;

			var dispatcherTimer = new DispatcherTimer
			{
				Interval = new TimeSpan(0, 0, 0, 0, 100)
			};

			dispatcherTimer.Tick += DispatcherTimer_Tick;
			dispatcherTimer.Start();
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

		public ExitCommand Exit { get; }

		public NewMockCommand NewMock { get; }

		public ClearMocksCommand ClearMocks { get; }

		public ObservableCollection<Mock> Mocks { get; }

		public ObservableCollection<HttpInteraction> HandledRequests { get; }

		public ClearHandledRequestsCommand ClearHandledRequests { get; }

		public ObservableCollection<HttpInteraction> UnhandledRequests { get; }

		public ClearUnhandledRequestsCommand ClearUnhandledRequests { get; }

		public ConnectionSettings ConnectionSettings { get; }

		public StartHttpServerCommand StartHttpServer { get; }

		public Visibility StartHttpServerVisibility { get; set; }

		public StopHttpServerCommand StopHttpServer { get; }

		public Visibility StopHttpServerVisibility { get; set; }

		public AboutProgramCommand AboutProgram { get; }

		public Mock SelectedMock
		{
			get => _selectedMock;
			set
			{
				_selectedMock = value;
				OnPropertyChanged(nameof(SelectedMock));
			}
		}

		public RemoveMockCommand RemoveMock { get; }

		public IEnumerable<string> Methods
		{
			get { return new[] { "GET", "POST" }; }
		}

		private void UpdateMocks()
		{
			if (_mockCache != null)
			{
				Mock[] mocks = _mockCache.GetAll().ToArray();

				var synchronizer = new MockCollectionSynchronizer();
				synchronizer.Synchronize(mocks, Mocks);
			}

			if (Mocks.All(mock => mock.Uid != SelectedMock?.Uid))
			{
				SelectedMock = null;
			}
		}
	}
}