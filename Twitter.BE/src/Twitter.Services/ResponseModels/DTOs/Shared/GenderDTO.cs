using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels.DTOs.Shared
{
    public class GenderDTO : IResponseDTO
    {
        public int GenderId { get; set; }
        public string GenderName { get; set; }
    }
}
