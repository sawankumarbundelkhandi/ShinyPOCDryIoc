using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Shiny;
using Shiny.Notifications;
using Shiny.Push;
using Xamarin.Essentials.Interfaces;

namespace ShinyPOCDryIoc.ViewModels
{
    public class PushPageViewModel : ViewModelBase
    {
        private string _token;
        private string _data;
        private IDisposable _whenReceivedSubscription;
        private readonly ISecureStorage _secureStorage;
        private readonly IPushManager _pushManager;
        private readonly INotificationManager _notificationManager;

        public PushPageViewModel(ISecureStorage secureStorage, IPushManager pushManager, INotificationManager notificationManager)
        {
            _secureStorage = secureStorage;
            _pushManager = pushManager;
            RegisterCommand = new DelegateCommand(async () => await RegisterCommandHandler());
            _notificationManager = notificationManager;
        }

        private async Task RegisterCommandHandler()
        {
            var result = await _pushManager.RequestAccess();

            var notificationResult = await _notificationManager.RequestAccess();

            if (result.Status == AccessState.Available && notificationResult == AccessState.Available)
            {
                await _secureStorage.SetAsync("token", result.RegistrationToken);
                Token = result.RegistrationToken;
            }
        }

        public DelegateCommand RegisterCommand { get; }

        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        public string Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Token = await _secureStorage.GetAsync("token");

            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                _whenReceivedSubscription = _pushManager.WhenReceived().SubscribeAsync(data =>
                {
                    Data = data.Notification.Title + data.Notification.Message;
                    return Task.CompletedTask;
                });

                if (parameters.ContainsKey("data"))
                {
                    Data = parameters.GetValue<string>("data");
                }
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            if(_whenReceivedSubscription != null)
            {
                _whenReceivedSubscription.Dispose();
            }
        }
    }
}