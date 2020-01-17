using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codity.Data.Model;
using Codity.Repositories.Interfaces;
using Codity.Services.Hubs;
using Codity.Services.Interfaces;
using Codity.Services.RequestModels;
using Codity.Services.Resources;
using Codity.Services.ResponseModels;
using Codity.Services.ResponseModels.DTOs.Notification;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Services
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
            var notifications = await _notificationRepository.GetPagedByAsync(
                getBy: c => c.UserId == userId,
                orderBy: c => c.Notification.CreatedTime,
                pageNumber: paginationRequest.PageNumber,
                pageSize: paginationRequest.PageSize,
                includes: c => c.Notification);
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
