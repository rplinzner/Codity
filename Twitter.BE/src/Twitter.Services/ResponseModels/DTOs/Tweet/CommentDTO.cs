using System;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels.DTOs.Tweet
{
    public class CommentDTO : IResponseDTO
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string AuthorImage { get; set; }

        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
    }
}
