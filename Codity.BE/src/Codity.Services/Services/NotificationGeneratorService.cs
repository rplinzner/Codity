using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codity.Data.Model;
using Codity.Data.Model.Enums;
using Codity.Repositories.Interfaces;
using Codity.Services.Helpers;
using Codity.Services.Helpers.NotificationParameters;
using Codity.Services.Interfaces;
using Codity.Services.Options;

namespace Codity.Services.Services
{
    public class NotificationGeneratorService : INotificationGeneratorService
    {
        private readonly INotificationMapperService _notificationMapper;
        private readonly INotificationService _notificationService;
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly IBaseRepository<UserNotification> _userNotificationRepository;
        private readonly IBaseRepository<Follow> _followRepository;
        private readonly RedirectOptions _redirectOptions;

        public NotificationGeneratorService(
            INotificationMapperService notificationMapper,
            INotificationService notificationService,
            IBaseRepository<Notification> notificationRepository,
            IBaseRepository<UserNotification> userNotificationRepository,
            IBaseRepository<Follow> followRepository,
            IOptions<RedirectOptions> redirectOptions)
        {
            _notificationMapper = notificationMapper;
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
            _followRepository = followRepository;
            _redirectOptions = redirectOptions.Value;
        }

        public async Task CreateCommentNotification(Post post, User commentAuthor, User postAuthor, string comment)
        {
            var commentAuthorFullName = $"{commentAuthor.FirstName} {commentAuthor.LastName}";
            var postAuthorFullName = $"{postAuthor.FirstName} {postAuthor.LastName}";

            var notificationParams = new CommentParameters()
            {
                CommentAuthorFullName = commentAuthorFullName,
                PostAuthorFullName = postAuthorFullName
            };

            var notification = new Notification
            {
                Parameters = _notificationMapper.SerializeNotificationParameters(notificationParams),
                PostId = post.Id,
                Type = NotificationType.Comment,
                Description = comment.Truncate(30),
                CreatedTime = DateTime.Now,
                RedirectTo = string.Format(_redirectOptions.PostUrl, post.Id)
            };

            await _notificationRepository.AddAsync(notification);

            var followers = (await _followRepository.GetAllByAsync(c => c.FollowingId == commentAuthor.Id)).Select(c => c.FollowerId);

            var userNotifications = new List<UserNotification>();
            foreach (var follower in followers)
            {
                userNotifications.Add(new UserNotification
                {
                    NotificationId = notification.Id,
                    UserId = follower
                });
            }

            if (commentAuthor.Id != postAuthor.Id)
            {
                var postAuthorNotificationParams = new CommentParameters()
                {
                    CommentAuthorFullName = commentAuthorFullName
                };

                var postAuthorNotification = new Notification
                {
                    Parameters = _notificationMapper.SerializeNotificationParameters(postAuthorNotificationParams),
                    PostId = post.Id,
                    Type = NotificationType.Comment,
                    Description = comment.Truncate(30),
                    CreatedTime = DateTime.Now,
                    RedirectTo = string.Format(_redirectOptions.PostUrl, post.Id)
                };

                await _notificationRepository.AddAsync(postAuthorNotification);

                userNotifications.Add(new UserNotification
                {
                    NotificationId = postAuthorNotification.Id,
                    UserId = postAuthor.Id
                });

                await _notificationService.SendNotification(postAuthor.Id, postAuthorNotification.Id);
            }

            await _userNotificationRepository.AddRangeAsync(userNotifications);

            await _notificationService.SendNotification(followers, new[] { commentAuthor.Id, postAuthor.Id }, notification.Id);
        }

        public async Task CreateFollowNotification(User follower, User following)
        {
            var followerFullName = $"{follower.FirstName} {follower.LastName}";
            var followingFullName = $"{following.FirstName} {following.LastName}";


            var notificationParams = new FollowerParameters()
            {
                FollowerFullName = followerFullName,
                FollowingFullName = followingFullName
            };

            var notification = new Notification
            {
                Parameters = _notificationMapper.SerializeNotificationParameters(notificationParams),
                Type = NotificationType.Follower,
                CreatedTime = DateTime.Now,
                RedirectTo = string.Format(_redirectOptions.UserUrl, following.Id)
            };

            await _notificationRepository.AddAsync(notification);

            var followers = (await _followRepository.GetAllByAsync(c => c.FollowingId == follower.Id)).Select(c => c.FollowerId);

            var userNotifications = new List<UserNotification>();
            foreach (var followerId in followers)
            {
                if (followerId != follower.Id && followerId != following.Id)
                {
                    userNotifications.Add(new UserNotification
                    {
                        NotificationId = notification.Id,
                        UserId = followerId
                    });
                }
            }

            var postAuthorNotificationParams = new FollowerParameters()
            {
                FollowerFullName = followerFullName
            };

            var followingNotification = new Notification
            {
                Parameters = _notificationMapper.SerializeNotificationParameters(postAuthorNotificationParams),
                Type = NotificationType.Follower,
                CreatedTime = DateTime.Now,
                RedirectTo = string.Format(_redirectOptions.UserUrl, follower.Id)
            };

            await _notificationRepository.AddAsync(followingNotification);

            userNotifications.Add(new UserNotification
            {
                NotificationId = followingNotification.Id,
                UserId = following.Id
            });

            await _notificationService.SendNotification(following.Id, followingNotification.Id);

            await _userNotificationRepository.AddRangeAsync(userNotifications);

            await _notificationService.SendNotification(followers, new[] { follower.Id, following.Id }, notification.Id);
        }

        public async Task CreateLikeNotification(Post post, User likeAuthor, User postAuthor)
        {
            var likeAuthorFullName = $"{likeAuthor.FirstName} {likeAuthor.LastName}";
            var postAuthorFullName = $"{postAuthor.FirstName} {postAuthor.LastName}";
            var distinctLikesCount = post.Likes
                .Where(c => c.UserId != likeAuthor.Id)
                .Count();

            var notifications = await _notificationRepository
                .GetAllByAsync(c => c.PostId == post.Id && c.Type == NotificationType.Like);

            var notification = notifications
                .FirstOrDefault(c => !string.IsNullOrEmpty(_notificationMapper.DeserializeNotificationParameters<LikeParameters>(c.Parameters).PostAuthorFullName));

            var notificationParams = new LikeParameters()
            {
                LikingUserFullName = likeAuthorFullName,
                PostAuthorFullName = postAuthorFullName,
                LikesCount = distinctLikesCount
            };

            if (notification == null)
            {
                notification = new Notification
                {
                    Parameters = _notificationMapper.SerializeNotificationParameters(notificationParams),
                    CreatedTime = DateTime.Now,
                    PostId = post.Id,
                    Type = NotificationType.Like,
                    RedirectTo = string.Format(_redirectOptions.PostUrl, post.Id)
                };

                await _notificationRepository.AddAsync(notification);
            }
            else
            {
                var oldNotifications = await _userNotificationRepository.GetAllByAsync(c => c.NotificationId == notification.Id);
                await _userNotificationRepository.RemoveRangeAsync(oldNotifications);

                notification.Parameters = _notificationMapper.SerializeNotificationParameters(notificationParams);
                notification.CreatedTime = DateTime.Now;
                await _notificationRepository.UpdateAsync(notification);
            }

            var followers = (await _followRepository.GetAllByAsync(c => c.FollowingId == likeAuthor.Id)).Select(c => c.FollowerId);

            var userNotifications = new List<UserNotification>();
            foreach (var follower in followers)
            {
                userNotifications.Add(new UserNotification
                {
                    NotificationId = notification.Id,
                    UserId = follower
                });
            }

            if (likeAuthor.Id != postAuthor.Id)
            {
                var postAuthorNotification = notifications
                    .FirstOrDefault(c => string.IsNullOrEmpty(_notificationMapper.DeserializeNotificationParameters<LikeParameters>(c.Parameters).PostAuthorFullName));

                var postAuthorNotificationParams = new LikeParameters()
                {
                    LikingUserFullName = likeAuthorFullName,
                    LikesCount = distinctLikesCount
                };

                if (postAuthorNotification == null)
                {
                    postAuthorNotification = new Notification
                    {

                        Parameters = _notificationMapper.SerializeNotificationParameters(postAuthorNotificationParams),
                        CreatedTime = DateTime.Now,
                        PostId = post.Id,
                        Type = NotificationType.Like,
                        RedirectTo = string.Format(_redirectOptions.PostUrl, post.Id)
                    };

                    await _notificationRepository.AddAsync(postAuthorNotification);
                }
                else
                {
                    var oldNotifications = await _userNotificationRepository.GetAllByAsync(c => c.NotificationId == postAuthorNotification.Id);
                    await _userNotificationRepository.RemoveRangeAsync(oldNotifications);

                    postAuthorNotification.Parameters = _notificationMapper.SerializeNotificationParameters(postAuthorNotificationParams);
                    postAuthorNotification.CreatedTime = DateTime.Now;
                    await _notificationRepository.UpdateAsync(postAuthorNotification);
                }

                userNotifications.Add(new UserNotification
                {
                    NotificationId = postAuthorNotification.Id,
                    UserId = postAuthor.Id,

                });

                await _notificationService.SendNotification(postAuthor.Id, postAuthorNotification.Id);
            }

            await _userNotificationRepository.AddRangeAsync(userNotifications);

            await _notificationService.SendNotification(followers, new[] { likeAuthor.Id, postAuthor.Id }, notification.Id);
        }

        public async Task CreatePostNotification(Post post, User postAuthor)
        {
            var postAuthorFullName = $"{postAuthor.FirstName} {postAuthor.LastName}";


            var notificationParams = new PostParameters()
            {
                AuthorFullName = postAuthorFullName,
            };

            var notification = new Notification
            {
                Parameters = _notificationMapper.SerializeNotificationParameters(notificationParams),
                PostId = post.Id,
                Type = NotificationType.Post,
                Description = post.Text.Truncate(30),
                CreatedTime = DateTime.Now,
                RedirectTo = string.Format(_redirectOptions.PostUrl, post.Id)
            };

            await _notificationRepository.AddAsync(notification);

            var followers = (await _followRepository.GetAllByAsync(c => c.FollowingId == postAuthor.Id)).Select(c => c.FollowerId);

            var userNotifications = new List<UserNotification>();
            foreach (var follower in followers)
            {
                userNotifications.Add(new UserNotification
                {
                    NotificationId = notification.Id,
                    UserId = follower
                });
            }

            await _userNotificationRepository.AddRangeAsync(userNotifications);

            await _notificationService.SendNotification(followers, new[] { postAuthor.Id }, notification.Id);
        }
    }
}
