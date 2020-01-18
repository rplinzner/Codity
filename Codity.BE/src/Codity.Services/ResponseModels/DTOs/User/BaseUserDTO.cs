using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.ResponseModels.DTOs.User
{
    public class BaseUserDTO : IResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public bool IsFollowing { get; set; }
        public int FollowersCount { get; set; }
    }
}
