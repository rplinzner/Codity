using System.Threading.Tasks;
using Codity.Services.RequestModels.User;
using Codity.Services.ResponseModels.DTOs.Settings;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Interfaces
{
    public interface ISettingsService
    {
        Task<IResponse<SettingsDTO>> GetSettingsAsync(int userId);
        Task<IBaseResponse> UpdateSettingsAsync(int userId, SettingsRequest following);
    }
}
