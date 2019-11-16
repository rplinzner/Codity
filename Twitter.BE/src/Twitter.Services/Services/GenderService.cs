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
