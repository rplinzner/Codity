using System;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.ResponseModels.DTOs.Post
{
    public class PostDTO : IResponseDTO
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string AuthorImage { get; set; }

        public DateTime CreationDate { get; set; }
        public string Text { get; set; }

        public int? CodeSnippetId { get; set; }
        public CodeSnippetDTO CodeSnippet { get; set; }

        public bool IsLiked { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
