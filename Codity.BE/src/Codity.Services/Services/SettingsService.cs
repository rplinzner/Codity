using AutoMapper;
using System.Threading.Tasks;
using Codity.Data.Model;
using Codity.Repositories.Interfaces;
using Codity.Services.Interfaces;
using Codity.Services.RequestModels.User;
using Codity.Services.ResponseModels;
using Codity.Services.ResponseModels.DTOs.Settings;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IBaseRepository<Settings> _settingsRepository;
        private readonly IBaseRepository<Language> _languageRepository;
        private readonly IMapper _mapper;

        public SettingsService(
            IBaseRepository<Settings> settingsRepository,
            IBaseRepository<Language> languageRepository,
            IMapper mapper)
        {
            _settingsRepository = settingsRepository;
            _languageRepository = languageRepository;
            _mapper = mapper;
        }

        public async Task<IResponse<SettingsDTO>> GetSettingsAsync(int userId)
        {
            var response = new Response<SettingsDTO>();

            var settings = await _settingsRepository.GetByAsync(c => c.UserId == userId, false, c => c.Language);

            response.Model = _mapper.Map<SettingsDTO>(settings);

            return response;
        }

        public async Task<IBaseResponse> UpdateSettingsAsync(int userId, SettingsRequest settingsRequest)
        {
            var response = new BaseResponse();

            var language = await _languageRepository.GetByAsync(c => c.Code == settingsRequest.LanguageCode);

            var settings = await _settingsRepository.GetByAsync(c => c.UserId == userId);

            if (settings == null)
            {
                settings = new Settings
                {
                    IsDarkTheme = settingsRequest.IsDarkTheme,
                    LanguageId = language.Id,
                    UserId = userId
                };

                await _settingsRepository.AddAsync(settings);
            }
            else
            {
                settings.IsDarkTheme = settingsRequest.IsDarkTheme;
                settings.LanguageId = language.Id;

                await _settingsRepository.UpdateAsync(settings);
            }

            return response;
        }
    }
}
