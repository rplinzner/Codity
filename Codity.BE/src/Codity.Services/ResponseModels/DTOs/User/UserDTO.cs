using System;
using System.Collections.Generic;
using Codity.Services.ResponseModels.DTOs.Post;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.ResponseModels.DTOs.User
{
    public class UserDTO : BaseUserDTO, IResponseDTO
    {
        public string AboutMe { get; set; }
        public DateTime? BirthDay { get; set; }
        public string GenderName { get; set; }
        public int FollowingCount { get; set; }
        public IEnumerable<PostDTO> LatestPosts { get; set; }
    }
}
