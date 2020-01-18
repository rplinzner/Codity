namespace Codity.Data.Model
{
    public class UserNotification : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int NotificationId { get; set; }
        public Notification Notification { get; set; }
    }
}
