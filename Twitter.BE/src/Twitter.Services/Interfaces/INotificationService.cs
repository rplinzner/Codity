using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Services.ResponseModels.DTOs.Notification;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotification(int userId, int notificationId);
        Task SendNotification(IEnumerable<int> userIds, IEnumerable<int> exceptUserIds, int notificationId);

        Task<IResponse<NotificationDTO>> GetNotification(int notificationId);
        Task<ICollectionResponse<NotificationDTO>> GetAllNotifications(int userId);
        Task<IBaseResponse> MarkNotificationAsRead(int notificationId);
        Task<IBaseResponse> MarkAllNotificationsAsRead();
    }
}
