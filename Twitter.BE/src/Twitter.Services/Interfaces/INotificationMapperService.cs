using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Services.Helpers.NotificationParameters;
using Twitter.Services.ResponseModels.DTOs.Notification;

namespace Twitter.Services.Interfaces
{
    public interface INotificationMapperService
    {
        NotificationDTO MapNotification(Notification notification);
        IEnumerable<NotificationDTO> MapNotifications(IEnumerable<Notification> notifications);
        T DeserializeNotificationParameters<T>(string parameters);
        string SerializeNotificationParameters(object parameters);
    }
}
