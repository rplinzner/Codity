using System.Collections.Generic;
using System.Threading.Tasks;
using Codity.Services.RequestModels;
using Codity.Services.ResponseModels.DTOs.Notification;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotification(int userId, int notificationId);
        Task SendNotification(IEnumerable<int> userIds, IEnumerable<int> exceptUserIds, int notificationId);

        Task<IResponse<NotificationDTO>> GetNotification(int notificationId);
        Task<IPagedResponse<NotificationDTO>> GetAllNotifications(int userId, PaginationRequest paingationRequest);
        Task<IBaseResponse> MarkNotificationAsRead(int notificationId);
        Task<IBaseResponse> MarkAllNotificationsAsRead();
    }
}
