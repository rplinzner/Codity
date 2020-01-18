using System;
using Codity.Data.Model.Enums;

namespace Codity.Data.Model
{
    public class Notification : BaseEntity
    {
        public int? PostId { get; set; }
        public Post Post { get; set; }

        public string RedirectTo { get; set; }
        public string Parameters { get; set; }
        public string Description { get; set; }
        public NotificationType Type { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
