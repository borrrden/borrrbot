using System;
using System.Collections.Generic;
using System.Text;

namespace borrrbot.API.OBS.NotificationsService
{
    public sealed class NotificationPushedEventArgs : EventArgs
    {
        public INotification Notification { get; }
    }

    public sealed class NotificationReadEventArgs : EventArgs
    {
        public IReadOnlyList<float> Counts { get; }
    }

    public interface INotificationsServiceApi
    {
        event EventHandler<NotificationPushedEventArgs> NotificationPushed;

        event EventHandler<NotificationReadEventArgs> NotificationRead;

        void ApplyAction(float notificationId);

        IReadOnlyList<INotification> GetAll(string type);

        INotification GetNotification(float number);

        IReadOnlyList<INotification> GetRead(string type);

        INotificationsSettings GetSettings();

        object GetSettingsFormData();

        IReadOnlyList<INotification> GetUnread(string type);

        void MarkAllAsRead();

        INotification Push(INotificationOptions notifyInfo);

        void RestoreDefaultSettings();

        void SetSettings(INotificationsSettings settings);

        void ShowNotifications();


    }
}
