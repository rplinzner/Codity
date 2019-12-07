using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels.DTOs.Settings
{
    public class SettingsDTO : IResponseDTO
    {
        public bool IsDarkTheme { get; set; }
        public string LanguageCode { get; set; }
    }
}
