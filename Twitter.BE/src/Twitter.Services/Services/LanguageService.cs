using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Repositories.Interfaces;
using Twitter.Services.Interfaces;
using Twitter.Services.ResponseModels;
using Twitter.Services.ResponseModels.DTOs.Shared;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IBaseRepository<Language> _languageRepository;
        private readonly IMapper _mapper;

        public LanguageService(
            IBaseRepository<Language> languageRepository,
            IMapper mapper)
        {
            _languageRepository = languageRepository;
            _mapper = mapper;
        }

        public async Task<ICollectionResponse<LanguageDTO>> GetLanguagesAsync()
        {
            var response = new CollectionResponse<LanguageDTO>();

            var languages = await _languageRepository.GetAllAsync();

            response.Models = _mapper.Map<IEnumerable<LanguageDTO>>(languages);

            return response;
        }
    }
}
