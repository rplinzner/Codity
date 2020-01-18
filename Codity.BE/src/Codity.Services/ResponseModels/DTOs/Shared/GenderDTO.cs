using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.ResponseModels.DTOs.Shared
{
    public class GenderDTO : IResponseDTO
    {
        public int GenderId { get; set; }
        public string GenderName { get; set; }
    }
}
