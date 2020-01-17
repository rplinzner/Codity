using System.Threading.Tasks;
using Codity.Services.RequestModels.Authentication;
using Codity.Services.ResponseModels.DTOs.Authentication;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IBaseResponse> RegisterAsync(RegisterRequest model);
        Task<IResponse<AuthUserDTO>> LoginAsync(LoginRequest model);
        Task<IBaseResponse> ChangePasswordAsync(int userId, ChangePasswordRequest model);
        Task<IBaseResponse> ForgetPasswordAsync(ForgetPasswordRequest model);
        Task<IBaseResponse> ResetPasswordAsync(ResetPasswordRequest model);
        Task<string> ConfirmEmailAsync(string id, string token);
    }
}
