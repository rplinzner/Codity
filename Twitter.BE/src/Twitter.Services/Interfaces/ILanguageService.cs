using System.Threading.Tasks;
using Twitter.Services.ResponseModels.DTOs.Shared;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Interfaces
{
    public interface ILanguageService
    {
        Task<ICollectionResponse<LanguageDTO>> GetLanguagesAsync();
    }
}
