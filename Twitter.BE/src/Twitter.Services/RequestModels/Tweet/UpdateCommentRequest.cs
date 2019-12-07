using System.ComponentModel.DataAnnotations;
using Twitter.Services.Resources;

namespace Twitter.Services.RequestModels.Tweet
{
    public class UpdateCommentRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public string Text { get; set; }
    }
}
