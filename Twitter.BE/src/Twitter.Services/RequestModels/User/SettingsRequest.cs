using System.ComponentModel.DataAnnotations;

namespace Twitter.Services.RequestModels.User
{
    public class SettingsRequest
    {
        [Required]
        public bool IsDarkTheme { get; set; }
        [Required]
        public string LanguageCode { get; set; }
    }
}
