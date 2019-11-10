using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<ICollectionResponse<NotificationDTO>>> GetUsers()
        {
            var userId = _userContext.GetUserId();

            var response = await _notificationService.GetAllNotifications(userId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}