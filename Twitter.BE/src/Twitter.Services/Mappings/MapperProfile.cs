using AutoMapper;
using Twitter.Data.Model;
using Twitter.Shared.RequestModels.Authentication;

namespace Twitter.Services.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterRequest, User>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));
        }
    }
}
