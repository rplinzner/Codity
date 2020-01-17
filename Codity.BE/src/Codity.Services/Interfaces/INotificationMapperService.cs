using System.Collections.Generic;
using Codity.Data.Model;
using Codity.Services.ResponseModels.DTOs.Notification;

namespace Codity.Services.Interfaces
{
    public interface INotificationMapperService
    {
        NotificationDTO MapNotification(Notification notification);
        IEnumerable<NotificationDTO> MapNotifications(IEnumerable<Notification> notifications);
        T DeserializeNotificationParameters<T>(string parameters);
        string SerializeNotificationParameters(object parameters);
    }
}
