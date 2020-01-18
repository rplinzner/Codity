using System;

namespace Codity.Data.Model
{
    public class Comment : BaseEntity
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
