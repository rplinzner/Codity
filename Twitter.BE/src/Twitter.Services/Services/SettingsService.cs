using AutoMapper;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Repositories.Interfaces;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels.User;
using Twitter.Services.ResponseModels;
using Twitter.Services.ResponseModels.DTOs.Settings;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IBaseRepository<Settings> _settingsRepository;
        private readonly IMapper _mapper;

        public SettingsService(
            IBaseRepository<Settings> settingsRepository,
            IMapper mapper)
        {
            _settingsRepository = settingsRepository;
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

            var settings = await _settingsRepository.GetByAsync(c => c.UserId == userId);

            _mapper.Map(settingsRequest, settings);

            await _settingsRepository.UpdateAsync(settings);

            return response;
        }
    }
}
