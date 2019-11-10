using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Repositories.Interfaces;
using Twitter.Services.Interfaces;
using Twitter.Services.ResponseModels;
using Twitter.Services.ResponseModels.DTOs.Notification;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationMapperService _notificationMapperService;
        private readonly IBaseRepository<UserNotification> _notificationRepository;

        public NotificationService(
            INotificationMapperService notificationMapperService,
            IBaseRepository<UserNotification> notificationRepository)
        {
            _notificationMapperService = notificationMapperService;
            _notificationRepository = notificationRepository;
        }

        public async Task<ICollectionResponse<NotificationDTO>> GetAllNotifications(int userId)
        {
            var response = new CollectionResponse<NotificationDTO>();

            var notifications = (await _notificationRepository
                .GetAllByAsync(c => c.UserId == userId, false, c => c.Notification))
                .Select(c => c.Notification);

            response.Models = _notificationMapperService.MapNotifications(notifications);

            return response;
        }

        public Task<IBaseResponse> MarkAllNotificationsAsRead()
        {
            throw new NotImplementedException();
        }

        public Task<IBaseResponse> MarkNotificationAsRead(int notificationId)
        {
            throw new NotImplementedException();
        }

        public async Task SendNotification(int userId, NotificationDTO notification)
        {
            Console.WriteLine($"{userId}");
            Console.WriteLine($"#{notification.Label} --- {notification.Description}");
        }

        public async Task SendNotification(IEnumerable<int> userIds, IEnumerable<int> exceptUserIds, NotificationDTO notification)
        {
            Console.WriteLine($"##{notification.Label} --- {notification.Description}");
        }
    }
}
