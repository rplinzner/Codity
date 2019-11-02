﻿namespace Twitter.Data.Model
{
    public class UserNotification : BaseEntity, IBaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int NotificationId { get; set; }
        public Notification Notification { get; set; }
    }
}
