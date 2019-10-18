using System;
using System.Collections.Generic;

namespace Twitter.Data.Model
{
    public class Tweet : BaseEntity
    {
        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int? CodeSnippetId { get; set; }
        public CodeSnippet CodeSnippet { get; set; }

        public DateTime CreationDate { get; set; }
        public string Text { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
