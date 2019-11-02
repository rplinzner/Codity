using System;
using Twitter.Services.ResponseModels.DTOs.Shared;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels.DTOs.User
{
    public class UserDTO : BaseUserDTO, IResponseDTO
    {
        public string AboutMe { get; set; }
        public DateTime? BirthDay { get; set; }
        public GenderDTO Gender { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
    }
}
