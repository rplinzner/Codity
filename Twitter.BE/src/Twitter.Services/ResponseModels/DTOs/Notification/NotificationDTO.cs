using System;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels.DTOs.Notification
{
    public class NotificationDTO : IResponseDTO
    {
        public string RedirectTo { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
