using System.ComponentModel.DataAnnotations;
using Twitter.Services.Resources;

namespace Twitter.Services.RequestModels.Tweet
{
    public class CommentRequest
    {
        [Required(ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public int TweetId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public string Text { get; set; }
    }
}
