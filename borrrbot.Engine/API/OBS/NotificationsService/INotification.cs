using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.NotificationsService
{
    public interface INotification : INotificationOptions
    {
        int date { get; }

        int id { get; }
    }
}
