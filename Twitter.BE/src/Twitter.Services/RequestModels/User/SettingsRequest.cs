using System.ComponentModel.DataAnnotations;
using Twitter.Services.Resources;

namespace Twitter.Services.RequestModels.User
{
    public class SettingsRequest
    {
        [Required(ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public bool IsDarkTheme { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public string LanguageCode { get; set; }
    }
}
