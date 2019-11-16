using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Twitter.Services.Interfaces;
using Twitter.Services.ResponseModels.DTOs.Notification;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<ICollectionResponse<NotificationDTO>>> GetNotifications()
        {
            var userId = _userContext.GetUserId();

            var response = await _notificationService.GetAllNotifications(userId);

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
        public async Task<ActionResult<ICollectionResponse<NotificationDTO>>> GetNotification(int id)
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