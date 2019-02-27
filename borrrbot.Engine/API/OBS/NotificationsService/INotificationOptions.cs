using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.NotificationsService
{
    public interface INotificationOptions
    {
        IJsonRcRequest action { get; }

        string code { get; }

        object data { get; }

        float lifeTime { get; }

        string message { get; }

        bool playSound { get; }

        bool showTime { get; }

        string type { get; }

        bool unread { get; }
    }
}
