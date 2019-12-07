using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels.DTOs.Tweet
{
    public class LikeUserDTO : IResponseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
