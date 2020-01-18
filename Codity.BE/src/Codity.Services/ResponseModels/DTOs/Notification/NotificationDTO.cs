using System;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.ResponseModels.DTOs.Notification
{
    public class NotificationDTO : IResponseDTO
    {
        public int Id { get; set; }
        public string RedirectTo { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
