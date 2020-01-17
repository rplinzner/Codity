using AutoMapper;
using Codity.Data.Model;
using Codity.Repositories.Helpers;
using Codity.Services.RequestModels.Authentication;
using Codity.Services.RequestModels.User;
using Codity.Services.ResponseModels.DTOs.Notification;
using Codity.Services.ResponseModels.DTOs.Settings;
using Codity.Services.ResponseModels.DTOs.Shared;
using Codity.Services.ResponseModels.DTOs.User;
using Codity.Services.ResponseModels;
using System.Linq;
using Codity.Services.RequestModels.Post;
using Microsoft.Extensions.Localization;
using Codity.Services.Resources;
using Codity.Services.ResponseModels.DTOs.Post;

namespace Codity.Services.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterRequest, User>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));

            CreateMap<User, BaseUserDTO>()
               .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count));

            CreateMap<User, UserDTO>()
               .ForMember(d => d.GenderName, o => o.MapFrom<TranslateResolver, string>(s => s.Gender == null ? null : s.Gender.Name))
               .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Following.Count));

            CreateMap<Gender, GenderDTO>()
                .ForMember(d => d.GenderId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.GenderName, o => o.MapFrom<TranslateResolver, string>(s => s.Name));

            CreateMap<Settings, SettingsDTO>()
                .ForMember(d => d.LanguageCode, o => o.MapFrom(s => s.Language.Code))
                .ForMember(d => d.HasGithubToken, o => o.MapFrom(s => !string.IsNullOrEmpty(s.GithubToken)));

            CreateMap<PagedList<User>, PagedResponse<BaseUserDTO>>()
                .ForMember(d => d.Models, o => o.MapFrom(s => s.ToList()));

            CreateMap<PagedList<Post>, PagedResponse<PostDTO>>()
                .ForMember(d => d.Models, o => o.MapFrom(s => s.ToList()));

            CreateMap<PagedList<Comment>, PagedResponse<CommentDTO>>()
                .ForMember(d => d.Models, o => o.MapFrom(s => s.ToList()));

            CreateMap<Post, PostDTO>()
               .ForMember(d => d.AuthorId, o => o.MapFrom(s => s.Author.Id))
               .ForMember(d => d.AuthorFirstName, o => o.MapFrom(s => s.Author.FirstName))
               .ForMember(d => d.AuthorLastName, o => o.MapFrom(s => s.Author.LastName))
               .ForMember(d => d.AuthorImage, o => o.MapFrom(s => s.Author.Image))
               .ForMember(d => d.LikesCount, o => o.MapFrom(s => s.Likes.Count))
               .ForMember(d => d.CommentsCount, o => o.MapFrom(s => s.Comments.Count));

            CreateMap<Comment, CommentDTO>()
               .ForMember(d => d.AuthorId, o => o.MapFrom(s => s.Author.Id))
               .ForMember(d => d.AuthorFirstName, o => o.MapFrom(s => s.Author.FirstName))
               .ForMember(d => d.AuthorLastName, o => o.MapFrom(s => s.Author.LastName))
               .ForMember(d => d.AuthorImage, o => o.MapFrom(s => s.Author.Image));


            CreateMap<CodeSnippet, CodeSnippetDTO>()
               .ForMember(d => d.ProgrammingLanguageName, o => o.MapFrom(s => s.ProgrammingLanguage.Name))
               .ForMember(d => d.ProgrammingLanguageCode, o => o.MapFrom(s => s.ProgrammingLanguage.Code));

            CreateMap<PagedList<Follow>, PagedResponse<BaseUserDTO>>();
            CreateMap<PagedList<UserNotification>, PagedResponse<NotificationDTO>>();
            CreateMap<PagedList<PostLike>, PagedResponse<LikeUserDTO>>();
            CreateMap<PostRequest, Post>();
            CreateMap<UpdatePostRequest, Post>();
            CreateMap<CodeSnippetRequest, CodeSnippet>();
            CreateMap<Notification, NotificationDTO>();
            CreateMap<UserProfileRequest, User>();
            CreateMap<User, LikeUserDTO>();
            CreateMap<ProgrammingLanguage, ProgrammingLanguageDTO>();
        }

        internal class TranslateResolver : IMemberValueResolver<object, object, string, string>
        {
            private readonly IStringLocalizer<NotificationTranslations> _localizer;
            public TranslateResolver(IStringLocalizer<NotificationTranslations> localizer)
            {
                _localizer = localizer;
            }

            public string Resolve(object source, object destination, string sourceMember, string destMember,
                ResolutionContext context)
            {
                if (string.IsNullOrEmpty(sourceMember))
                {
                    return null;
                }

                return _localizer[sourceMember];
            }
        }
    }
}
