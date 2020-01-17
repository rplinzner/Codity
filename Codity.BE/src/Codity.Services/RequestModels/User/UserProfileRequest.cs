using System;

namespace Codity.Services.RequestModels.User
{
    public class UserProfileRequest
    {
        public string Image { get; set; }
        public string AboutMe { get; set; }
        public DateTime? BirthDay { get; set; }
        public int? GenderId { get; set; }
    }
}
