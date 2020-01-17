using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.ResponseModels.DTOs.Authentication
{
    public class AuthUserDTO : IResponseDTO
    {
        public int Id { get; set; }
        public string Token { get; set; }
    }
}
