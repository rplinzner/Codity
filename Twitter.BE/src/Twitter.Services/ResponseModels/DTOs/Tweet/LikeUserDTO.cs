using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels.DTOs.Tweet
{
    public class LikeUserDTO : IResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
    }
}
