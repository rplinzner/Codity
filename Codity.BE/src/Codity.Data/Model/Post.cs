using System;
using System.Collections.Generic;

namespace Codity.Data.Model
{
    public class Post : BaseEntity
    {
        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int? CodeSnippetId { get; set; }
        public CodeSnippet CodeSnippet { get; set; }

        public DateTime CreationDate { get; set; }
        public string Text { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<PostLike> Likes { get; set; }
    }
}
