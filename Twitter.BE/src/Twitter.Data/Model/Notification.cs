using System;

namespace Twitter.Data.Model
{
    public class Notification : BaseEntity, IBaseEntity
    {
        public string RedirectTo { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
