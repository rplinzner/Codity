using System;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels.DTOs.Tweet
{
    public class CommentDTO : IResponseDTO
    {
        public int Id { get; set; }

        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }

        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
    }
}
