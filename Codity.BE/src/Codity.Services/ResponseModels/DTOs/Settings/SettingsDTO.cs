using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.ResponseModels.DTOs.Settings
{
    public class SettingsDTO : IResponseDTO
    {
        public bool IsDarkTheme { get; set; }
        public string LanguageCode { get; set; }
        public bool HasGithubToken { get; set; }
    }
}
