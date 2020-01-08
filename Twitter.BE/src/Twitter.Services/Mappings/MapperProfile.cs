using AutoMapper;
using Twitter.Data.Model;
using Twitter.Repositories.Helpers;
using Twitter.Services.RequestModels.Authentication;
using Twitter.Services.RequestModels.User;
using Twitter.Services.ResponseModels.DTOs.Notification;
using Twitter.Services.ResponseModels.DTOs.Settings;
using Twitter.Services.ResponseModels.DTOs.Shared;
using Twitter.Services.ResponseModels.DTOs.User;
using Twitter.Services.ResponseModels;
using System.Linq;
using Twitter.Services.RequestModels.Tweet;
using Twitter.Services.ResponseModels.DTOs.Tweet;
using Microsoft.Extensions.Localization;
using Twitter.Services.Resources;

namespace Twitter.Services.Mappings
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

            CreateMap<PagedList<Tweet>, PagedResponse<TweetDTO>>()
                .ForMember(d => d.Models, o => o.MapFrom(s => s.ToList()));

            CreateMap<PagedList<Comment>, PagedResponse<CommentDTO>>()
                .ForMember(d => d.Models, o => o.MapFrom(s => s.ToList()));

            CreateMap<Tweet, TweetDTO>()
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
               .ForMember(d => d.ProgrammingLanguageName, o => o.MapFrom(s => s.ProgrammingLanguage.Name));

            CreateMap<PagedList<Follow>, PagedResponse<BaseUserDTO>>();
            CreateMap<PagedList<UserNotification>, PagedResponse<NotificationDTO>>();
            CreateMap<PagedList<TweetLike>, PagedResponse<LikeUserDTO>>();
            CreateMap<TweetRequest, Tweet>();
            CreateMap<UpdateTweetRequest, Tweet>();
            CreateMap<CodeSnippetRequest, CodeSnippet>();
            CreateMap<Notification, NotificationDTO>();
            CreateMap<UserProfileRequest, User>();
            CreateMap<User, LikeUserDTO>();
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
