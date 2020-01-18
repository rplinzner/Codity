using System.ComponentModel.DataAnnotations;
using Codity.Services.Resources;

namespace Codity.Services.RequestModels.Post
{
    public class UpdateCommentRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public string Text { get; set; }
    }
}
