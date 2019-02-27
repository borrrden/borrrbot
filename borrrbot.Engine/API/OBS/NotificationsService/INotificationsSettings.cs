using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.NotificationsService
{
    public interface INotificationsSettings
    {
        bool enabled { get; }

        bool playSound { get; }
    }
}
