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
    public class GenderService : IGenderService
    {
        private readonly IBaseRepository<Gender> _genderRepository;
        private readonly IMapper _mapper;

        public GenderService(
            IBaseRepository<Gender> genderRepository,
            IMapper mapper)
        {
            _genderRepository = genderRepository;
            _mapper = mapper;
        }

        public async Task<ICollectionResponse<GenderDTO>> GetGendersAsync()
        {
            var response = new CollectionResponse<GenderDTO>();

            var genders = await _genderRepository.GetAllAsync();

            response.Models = _mapper.Map<IEnumerable<GenderDTO>>(genders);

            return response;
        }
    }
}
