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
    public class ProgrammingLanguageService : IProgrammingLanguageService
    {
        private readonly IBaseRepository<ProgrammingLanguage> _programmingLanguageRepository;
        private readonly IMapper _mapper;

        public ProgrammingLanguageService(
            IBaseRepository<ProgrammingLanguage> programmingLanguageRepository,
            IMapper mapper)
        {
            _programmingLanguageRepository = programmingLanguageRepository;
            _mapper = mapper;
        }

        public async Task<ICollectionResponse<ProgrammingLanguageDTO>> GetProgrammingLanguagesAsync()
        {
            var response = new CollectionResponse<ProgrammingLanguageDTO>();

            var programmingLanguages = await _programmingLanguageRepository.GetAllAsync();

            response.Models = _mapper.Map<IEnumerable<ProgrammingLanguageDTO>>(programmingLanguages);

            return response;
        }
    }
}
