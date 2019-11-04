using System.ComponentModel.DataAnnotations;

namespace Twitter.Services.RequestModels.User
{
    public class FollowingRequest
    {
        [Required]
        public int FollowingId { get; set; }
    }
}
