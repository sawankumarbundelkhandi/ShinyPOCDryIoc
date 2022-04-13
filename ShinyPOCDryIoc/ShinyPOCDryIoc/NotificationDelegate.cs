using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Shiny.Notifications;

namespace ShinyPOCDryIoc
{
    public class NotificationDelegate : INotificationDelegate
    {
        public NotificationDelegate()
        {
        }

        public Task OnEntry(NotificationResponse response)
        {
            Debug.Write("NotificationDelegateOnEntry" + response);
            return Task.CompletedTask;
        }
    }
}
