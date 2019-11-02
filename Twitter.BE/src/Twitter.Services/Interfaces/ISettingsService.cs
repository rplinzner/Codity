using System.Threading.Tasks;
using Twitter.Services.RequestModels.User;
using Twitter.Services.ResponseModels.DTOs.Settings;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Interfaces
{
    public interface ISettingsService
    {
        Task<IResponse<SettingsDTO>> GetSettingsAsync(int userId);
        Task<IBaseResponse> UpdateSettingsAsync(int userId, SettingsRequest following);
    }
}
