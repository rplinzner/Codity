using System.ComponentModel.DataAnnotations;
using Twitter.Services.Resources;

namespace Twitter.Services.RequestModels.Authentication
{
    public class ChangePasswordRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public string OldPassword { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public string NewPassword { get; set; }
    }
}
