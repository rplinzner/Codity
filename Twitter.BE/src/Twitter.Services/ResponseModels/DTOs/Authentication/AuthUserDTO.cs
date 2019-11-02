using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels.DTOs.Authentication
{
    public class AuthUserDTO : IResponseDTO
    {
        public int Id { get; set; }
        public string Token { get; set; }
    }
}
