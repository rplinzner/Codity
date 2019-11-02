using System.Threading.Tasks;
using Twitter.Services.RequestModels.Authentication;
using Twitter.Services.ResponseModels.DTOs.Authentication;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IBaseResponse> RegisterAsync(RegisterRequest model);
        Task<IResponse<AuthUserDTO>> LoginAsync(LoginRequest model);
        Task<string> ConfirmEmailAsync(string id, string token);
    }
}
