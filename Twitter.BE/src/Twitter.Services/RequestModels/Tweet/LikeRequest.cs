using System.ComponentModel.DataAnnotations;
using Twitter.Services.Resources;

namespace Twitter.Services.RequestModels.Tweet
{
    public class LikeRequest
    {
        [Required(ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public int TweetId { get; set; }
    }
}
