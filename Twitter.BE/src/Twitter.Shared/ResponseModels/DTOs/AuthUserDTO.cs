using Twitter.Shared.ResponseModels.Interfaces;

namespace Twitter.Shared.ResponseModels.DTOs
{
    public class AuthUserDTO: IResponseDTO
    {
        public int Id { get; set; }
        public string Token { get; set; }
    }
}
