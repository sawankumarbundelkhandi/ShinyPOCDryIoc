using System.Diagnostics;
using System.Threading.Tasks;
using Shiny.Push;
using Xamarin.Essentials.Interfaces;

namespace ShinyPOCDryIoc
{
    public class PushDelegate : IPushDelegate
    {
        private readonly ISecureStorage _secureStorage;

        public PushDelegate(ISecureStorage secureStorage)
        {
            _secureStorage = secureStorage;
        }

        public Task OnEntry(PushNotificationResponse response)
        {
            Debug.Write("OnEntry" + response);
            return Task.CompletedTask;
        }

        public Task OnReceived(PushNotification notification)
        {
            Debug.Write("OnReceived" + notification);
            return Task.CompletedTask;
        }

        public async Task OnTokenRefreshed(string token)
        {
            Debug.Write("OnTokenRefreshed" + token);
            await _secureStorage.SetAsync("token", token);
        }
    }
}