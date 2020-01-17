using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Codity.Services.Interfaces;
using Codity.Services.RequestModels;
using Codity.Services.ResponseModels.DTOs.Notification;
using Codity.Services.ResponseModels.Interfaces;
using Codity.WebApi.Filters;

namespace Codity.WebApi.Controllers
{
    [Route("api/[controller]")]
    [BaseFilter]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IUserContext _userContext;

        public NotificationController(INotificationService notificationService, IUserContext userContext)
        {
            _notificationService = notificationService;
            _userContext = userContext;
        }

        /// <summary>
        /// Fetch all notifications
        /// </summary>
        /// <returns>All Notifications for logged user</returns>
        [HttpGet]
        public async Task<ActionResult<IPagedResponse<NotificationDTO>>> GetNotifications([FromQuery] PaginationRequest paginationRequest)
        {
            var userId = _userContext.GetUserId();

            var response = await _notificationService.GetAllNotifications(userId, paginationRequest);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Fetch notification
        /// </summary>
        /// <returns>Notification</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IResponse<NotificationDTO>>> GetNotification(int id)
        {
            var response = await _notificationService.GetNotification(id);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}