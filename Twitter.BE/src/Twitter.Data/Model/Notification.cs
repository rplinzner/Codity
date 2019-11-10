using System;
using Twitter.Data.Model.Enums;

namespace Twitter.Data.Model
{
    public class Notification : BaseEntity, IBaseEntity
    {
        public int? TweetId { get; set; }
        public Tweet Tweet { get; set; }

        public string RedirectTo { get; set; }
        public string Parameters { get; set; }
        public string Description { get; set; }
        public NotificationType Type { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
