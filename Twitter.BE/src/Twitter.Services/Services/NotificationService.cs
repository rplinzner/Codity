using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Repositories.Interfaces;
using Twitter.Services.Hubs;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels;
using Twitter.Services.Resources;
using Twitter.Services.ResponseModels;
using Twitter.Services.ResponseModels.DTOs.Notification;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationMapperService _notificationMapperService;
        private readonly IBaseRepository<UserNotification> _notificationRepository;
        private readonly IHubContext<NotificationHub> _notificationContext;
        private readonly IMapper _mapper;

        public NotificationService(
            INotificationMapperService notificationMapperService,
            IBaseRepository<UserNotification> notificationRepository,
            IHubContext<NotificationHub> notificationContext,
            IMapper mapper)
        {
            _notificationMapperService = notificationMapperService;
            _notificationRepository = notificationRepository;
            _notificationContext = notificationContext;
            _mapper = mapper;
        }

        public async Task<IPagedResponse<NotificationDTO>> GetAllNotifications(int userId, PaginationRequest paginationRequest)
        {
            var response = new PagedResponse<NotificationDTO>();
            var notifications = await _notificationRepository
                .GetPagedByAsync(c => c.UserId == userId, paginationRequest.PageNumber, paginationRequest.PageSize, false, c => c.Notification);
            _mapper.Map(notifications, response);

            response.Models = _notificationMapperService.MapNotifications(notifications.Select(c => c.Notification));

            return response;
        }

        public async Task<IResponse<NotificationDTO>> GetNotification(int notificationId)
        {
            var response = new Response<NotificationDTO>();

            var notification = await _notificationRepository
                .GetByAsync(c => c.NotificationId == notificationId, false, c => c.Notification);

            if (notification == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.NotificationNotFound
                });

                return response;
            }

            response.Model = _notificationMapperService.MapNotification(notification.Notification);

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

        public async Task SendNotification(int userId, int notificationId)
        {
            await _notificationContext.Clients.User(userId.ToString()).SendAsync("newNotification", notificationId);
        }

        public async Task SendNotification(IEnumerable<int> userIds, IEnumerable<int> exceptUserIds, int notificationId)
        {
            var users = userIds.Except(exceptUserIds).Select(c => c.ToString()).ToList();
            await _notificationContext.Clients.Users(users).SendAsync("newNotification", notificationId);
        }
    }
}
