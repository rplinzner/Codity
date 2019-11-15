using AutoMapper;
using Twitter.Data.Model;
using Twitter.Services.RequestModels.Authentication;
using Twitter.Services.RequestModels.User;
using Twitter.Services.ResponseModels.DTOs.Notification;
using Twitter.Services.ResponseModels.DTOs.Settings;
using Twitter.Services.ResponseModels.DTOs.Shared;
using Twitter.Services.ResponseModels.DTOs.User;

namespace Twitter.Services.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterRequest, User>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));

            CreateMap<User, BaseUserDTO>();

            CreateMap<User, UserDTO>()
               .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
               .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Following.Count));

            CreateMap<Gender, GenderDTO>();
            
            CreateMap<Notification, NotificationDTO>();

            CreateMap<Language, LanguageDTO>()
               .ForMember(d => d.LanguageId, o => o.MapFrom(s => s.Id))
               .ForMember(d => d.LanguageName, o => o.MapFrom(s => s.Name));

            CreateMap<SettingsRequest, Settings>();
            CreateMap<Settings, SettingsDTO>();
        }
    }
}
