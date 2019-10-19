using System.Threading.Tasks;
using Twitter.Shared.RequestModels.Authentication;
using Twitter.Shared.ResponseModels.DTOs;
using Twitter.Shared.ResponseModels.Interfaces;

namespace Twitter.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IBaseResponse> RegisterAsync(RegisterRequest model);
        Task<IResponse<AuthUserDTO>> LoginAsync(LoginRequest model);
        Task<string> ConfirmEmailAsync(string id, string token);
    }
}
