using System.ComponentModel.DataAnnotations;
using Codity.Services.Resources;

namespace Codity.Services.RequestModels.Post
{
    public class CommentRequest
    {
        [Required(ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public int PostId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public string Text { get; set; }
    }
}
