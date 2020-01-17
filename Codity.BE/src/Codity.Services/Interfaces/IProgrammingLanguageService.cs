using System.Threading.Tasks;
using Codity.Services.ResponseModels.DTOs.Shared;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Interfaces
{
    public interface IProgrammingLanguageService
    {
        Task<ICollectionResponse<ProgrammingLanguageDTO>> GetProgrammingLanguagesAsync();
    }
}
