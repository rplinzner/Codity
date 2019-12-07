using System.ComponentModel.DataAnnotations;
using Twitter.Services.Resources;

namespace Twitter.Services.RequestModels.User
{
    public class FollowingRequest
    {
        [Required(ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public int FollowingId { get; set; }
    }
}
