using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Codity.Data.Model;
using Codity.Repositories.Interfaces;
using Codity.Services.Interfaces;
using Codity.Services.ResponseModels;
using Codity.Services.ResponseModels.DTOs.Shared;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Services
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
