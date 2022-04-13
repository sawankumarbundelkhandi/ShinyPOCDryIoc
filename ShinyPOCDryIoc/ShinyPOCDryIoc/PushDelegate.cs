using System.Diagnostics;
using System.Threading.Tasks;
using Shiny.Notifications;
using Shiny.Push;
using Xamarin.Essentials.Interfaces;

namespace ShinyPOCDryIoc
{
    public class PushDelegate : IPushDelegate
    {
        private readonly ISecureStorage _secureStorage;
        private readonly INotificationManager _notificationManager;

        public PushDelegate(ISecureStorage secureStorage, INotificationManager notificationManager)
        {
            _secureStorage = secureStorage;
            _notificationManager = notificationManager;
        }

        public Task OnEntry(PushNotificationResponse response)
        {
            Debug.Write("OnEntry" + response);
            return Task.CompletedTask;
        }

        public Task OnReceived(PushNotification notification)
        {
            Debug.Write("OnReceived" + notification);
            _notificationManager.Send(new Notification()
            {
                Title = notification.Notification.Title,
                Message = notification.Notification.Message
            });
            return Task.CompletedTask;
        }

        public async Task OnTokenRefreshed(string token)
        {
            Debug.Write("OnTokenRefreshed" + token);
            await _secureStorage.SetAsync("token", token);
        }
    }
}