using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Twitter.Services.ResponseModels.DTOs.Notification;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotification(int userId, NotificationDTO notification);
        Task SendNotification(IEnumerable<int> userIds, IEnumerable<int> exceptUserIds, NotificationDTO notification);

        Task<ICollectionResponse<NotificationDTO>> GetAllNotifications(int userId);
        Task<IBaseResponse> MarkNotificationAsRead(int notificationId);
        Task<IBaseResponse> MarkAllNotificationsAsRead();
    }
}
