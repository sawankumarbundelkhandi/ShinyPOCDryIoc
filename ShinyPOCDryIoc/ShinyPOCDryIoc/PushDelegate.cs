using System.Diagnostics;
using System.Threading.Tasks;
using Prism.Navigation;
using Shiny.Push;
using ShinyPOCDryIoc.Views;
using Xamarin.Essentials.Interfaces;

namespace ShinyPOCDryIoc
{
    public class PushDelegate : IPushDelegate
    {
        private readonly INavigationService _navigationService;
        private readonly ISecureStorage _secureStorage;

        public PushDelegate(INavigationService navigationService, ISecureStorage secureStorage)
        {
            _navigationService = navigationService;
            _secureStorage = secureStorage;
        }

        public async Task OnEntry(PushNotificationResponse response)
        {
            Debug.Write("OnEntry" + response);
        }

        public async Task OnReceived(PushNotification notification)
        {
            Debug.Write("OnReceived" + notification);
            await _navigationService.NavigateAsync(nameof(PushPage), ("data", notification.Data.ToString()));
        }

        public async Task OnTokenRefreshed(string token)
        {
            Debug.Write("OnTokenRefreshed" + token);
            await _secureStorage.SetAsync("token", token);
        }
    }
}