using AutoMapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Codity.Data.Model;
using Codity.Data.Model.Enums;
using Codity.Services.Helpers.NotificationParameters;
using Codity.Services.Interfaces;
using Codity.Services.Resources;
using Codity.Services.ResponseModels.DTOs.Notification;

namespace Codity.Services.Services
{
    public class NotificationMapperService : INotificationMapperService
    {
        private readonly IMapper _mapper;

        public NotificationMapperService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public NotificationDTO MapNotification(Notification notification)
        {
            switch (notification.Type)
            {
                case NotificationType.Post:
                    {
                        return MapPostNotification(notification);
                    }
                case NotificationType.Like:
                    {
                        return MapLikeNotification(notification);
                    }
                case NotificationType.Follower:
                    {
                        return MapFollowerNotification(notification);
                    }
                case NotificationType.Comment:
                    {
                        return MapCommentNotification(notification);
                    }
                default:
                    {
                        return new NotificationDTO();
                    }
            }
        }

        public IEnumerable<NotificationDTO> MapNotifications(IEnumerable<Notification> notifications)
        {
            return notifications.Select(MapNotification);
        }

        private NotificationDTO MapCommentNotification(Notification notification)
        {
            var dto = new NotificationDTO();
            var commentNotificationParameters =
                DeserializeNotificationParameters<CommentParameters>(notification.Parameters);

            if (string.IsNullOrEmpty(commentNotificationParameters.PostAuthorFullName))
            {
                dto.Label = string.Format(
                    NotificationTranslations.YoursNewComment,
                    commentNotificationParameters.CommentAuthorFullName);
            }
            else
            {
                dto.Label = string.Format(
                    NotificationTranslations.NewComment,
                    commentNotificationParameters.CommentAuthorFullName,
                    commentNotificationParameters.PostAuthorFullName);
            }

            _mapper.Map(notification, dto);

            return dto;
        }

        private NotificationDTO MapFollowerNotification(Notification notification)
        {
            var dto = new NotificationDTO();
            var followerNotificationParameters =
                DeserializeNotificationParameters<FollowerParameters>(notification.Parameters);

            if (string.IsNullOrEmpty(followerNotificationParameters.FollowingFullName))
            {
                dto.Label = string.Format(
                    NotificationTranslations.YoursNewFollower,
                    followerNotificationParameters.FollowerFullName);
            }
            else
            {
                dto.Label = string.Format(
                    NotificationTranslations.NewFollower,
                    followerNotificationParameters.FollowerFullName,
                    followerNotificationParameters.FollowingFullName);
            }

            _mapper.Map(notification, dto);

            return dto;
        }

        private NotificationDTO MapLikeNotification(Notification notification)
        {
            var dto = new NotificationDTO();
            var likeNotificationParameters =
                DeserializeNotificationParameters<LikeParameters>(notification.Parameters);

            if (string.IsNullOrEmpty(likeNotificationParameters.PostAuthorFullName))
            {
                if (likeNotificationParameters.LikesCount > 0)
                {
                    dto.Label = string.Format(
                        NotificationTranslations.YoursNewLikes,
                        likeNotificationParameters.LikingUserFullName,
                        likeNotificationParameters.LikesCount);
                }
                else
                {
                    dto.Label = string.Format(
                        NotificationTranslations.YoursNewLike,
                        likeNotificationParameters.LikingUserFullName);
                }
            }
            else
            {
                dto.Label = string.Format(
                    NotificationTranslations.NewLike,
                    likeNotificationParameters.LikingUserFullName,
                    likeNotificationParameters.PostAuthorFullName);
            }

            _mapper.Map(notification, dto);

            return dto;
        }

        private NotificationDTO MapPostNotification(Notification notification)
        {
            var dto = new NotificationDTO();
            var postNotificationParameters =
                DeserializeNotificationParameters<PostParameters>(notification.Parameters);

            dto.Label = string.Format(
                NotificationTranslations.NewPost,
                postNotificationParameters.AuthorFullName);

            _mapper.Map(notification, dto);

            return dto;
        }

        public T DeserializeNotificationParameters<T>(string parameters)
        {
            return JsonConvert.DeserializeObject<T>(parameters);
        }

        public string SerializeNotificationParameters(object parameters)
        {
            return JsonConvert.SerializeObject(parameters);
        }
    }
}
