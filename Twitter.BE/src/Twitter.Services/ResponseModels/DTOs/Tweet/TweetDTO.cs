using System;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels.DTOs.Tweet
{
    public class TweetDTO : IResponseDTO
    {
        public int Id { get; set; }

        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }

        public DateTime CreationDate { get; set; }
        public string Text { get; set; }

        public int? CodeSnippetId { get; set; }
        public CodeSnippetDTO CodeSnippet { get; set; }

        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
