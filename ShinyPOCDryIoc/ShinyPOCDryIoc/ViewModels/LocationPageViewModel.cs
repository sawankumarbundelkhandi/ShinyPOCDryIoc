using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Shiny;
using Shiny.Locations;
using Xamarin.CommunityToolkit.ObjectModel;

namespace ShinyPOCDryIoc.ViewModels
{
    public class LocationPageViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IGpsManager _gpsManager;

        public LocationPageViewModel(IEventAggregator eventAggregator, IGpsManager gpsManager)
        {
            _eventAggregator = eventAggregator;
            _gpsManager = gpsManager;
            Title = "Location";
            StartListingCommand = new DelegateCommand(async () => await StartListingCommandHandler());
        }

        private async Task StartListingCommandHandler()
        {
            if (_gpsManager.IsListening())
            {
                await _gpsManager.StopListener();
                RaisePropertyChanged(nameof(StartOrStop));
                return;
            }

            var result = await _gpsManager.RequestAccess(GpsRequest.Realtime(true));

            if (result == AccessState.Available)
            {
                await _gpsManager.StartListener(GpsRequest.Realtime(true));
            }

            RaisePropertyChanged(nameof(StartOrStop));
        }

        public DelegateCommand StartListingCommand { get; }

        public ObservableRangeCollection<string> Locations { get; } = new ObservableRangeCollection<string>();

        public string StartOrStop => _gpsManager.IsListening() ? "Stop" : "Start";

        private void LocationUpdateEventHandler(string location)
        {
            Locations.Add(location);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            _eventAggregator.GetEvent<LocationUpdateEvent>().Subscribe(LocationUpdateEventHandler, ThreadOption.UIThread);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            _eventAggregator.GetEvent<LocationUpdateEvent>().Unsubscribe(LocationUpdateEventHandler);
        }
    }
    public class LocationUpdateEvent : PubSubEvent<string> { }


}