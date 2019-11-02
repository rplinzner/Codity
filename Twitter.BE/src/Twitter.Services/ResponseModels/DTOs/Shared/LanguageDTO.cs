using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels.DTOs.Shared
{
    public class LanguageDTO : IResponseDTO
    {
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
    }
}
