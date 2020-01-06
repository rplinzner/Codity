using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Data.Model.Enums;
using Twitter.Repositories.Interfaces;
using Twitter.Services.Helpers;
using Twitter.Services.Helpers.NotificationParameters;
using Twitter.Services.Interfaces;
using Twitter.Services.Options;

namespace Twitter.Services.Services
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

        public async Task CreateCommentNotification(Tweet tweet, User commentAuthor, User tweetAuthor, string comment)
        {
            var commentAuthorFullName = $"{commentAuthor.FirstName} {commentAuthor.LastName}";
            var tweetAuthorFullName = $"{tweetAuthor.FirstName} {tweetAuthor.LastName}";

            var notification = new Notification();

            var notificationParams = new CommentParameters()
            {
                CommentAuthorFullName = commentAuthorFullName,
                TweetAuthorFullName = tweetAuthorFullName
            };

            notification.Parameters = _notificationMapper.SerializeNotificationParameters(notificationParams);
            notification.TweetId = tweet.Id;
            notification.Type = NotificationType.Comment;
            notification.Description = comment.Truncate(30);
            notification.CreatedTime = DateTime.Now;
            notification.RedirectTo = string.Format(_redirectOptions.TweetUrl, tweet.Id);

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

            if (commentAuthor.Id != tweetAuthor.Id)
            {
                var tweetAuthorNotification = new Notification();
                var tweetAuthorNotificationParams = new CommentParameters()
                {
                    CommentAuthorFullName = commentAuthorFullName
                };
                tweetAuthorNotification.Parameters = _notificationMapper.SerializeNotificationParameters(tweetAuthorNotificationParams);
                tweetAuthorNotification.TweetId = tweet.Id;
                tweetAuthorNotification.Type = NotificationType.Comment;
                tweetAuthorNotification.Description = comment.Truncate(30);
                tweetAuthorNotification.CreatedTime = DateTime.Now;
                tweetAuthorNotification.RedirectTo = string.Format(_redirectOptions.TweetUrl, tweet.Id);

                await _notificationRepository.AddAsync(tweetAuthorNotification);

                userNotifications.Add(new UserNotification
                {
                    NotificationId = tweetAuthorNotification.Id,
                    UserId = tweetAuthor.Id
                });

                await _notificationService.SendNotification(tweetAuthor.Id, tweetAuthorNotification.Id);
            }

            await _userNotificationRepository.AddRangeAsync(userNotifications);

            await _notificationService.SendNotification(followers, new[] { commentAuthor.Id, tweetAuthor.Id }, notification.Id);
        }

        public async Task CreateFollowNotification(User follower, User following)
        {
            var followerFullName = $"{follower.FirstName} {follower.LastName}";
            var followingFullName = $"{following.FirstName} {following.LastName}";

            var notification = new Notification();

            var notificationParams = new FollowerParameters()
            {
                FollowerFullName = followerFullName,
                FollowingFullName = followingFullName
            };

            notification.Parameters = _notificationMapper.SerializeNotificationParameters(notificationParams);
            notification.Type = NotificationType.Follower;
            notification.CreatedTime = DateTime.Now;
            notification.RedirectTo = string.Format(_redirectOptions.UserUrl, following.Id);

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

            var followingNotification = new Notification();
            var tweetAuthorNotificationParams = new FollowerParameters()
            {
                FollowerFullName = followerFullName
            };
            followingNotification.Parameters = _notificationMapper.SerializeNotificationParameters(tweetAuthorNotificationParams);
            followingNotification.Type = NotificationType.Follower;
            followingNotification.CreatedTime = DateTime.Now;
            followingNotification.RedirectTo = string.Format(_redirectOptions.UserUrl, follower.Id);

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

        public async Task CreateLikeNotification(Tweet tweet, User likeAuthor, User tweetAuthor)
        {
            var likeAuthorFullName = $"{likeAuthor.FirstName} {likeAuthor.LastName}";
            var tweetAuthorFullName = $"{tweetAuthor.FirstName} {tweetAuthor.LastName}";
            var distinctLikesCount = tweet.Likes
                .Where(c => c.UserId != likeAuthor.Id)
                .Count();

            var notifications = await _notificationRepository
                .GetAllByAsync(c => c.TweetId == tweet.Id && c.Type == NotificationType.Like);

            var notification = notifications
                .FirstOrDefault(c => !string.IsNullOrEmpty(_notificationMapper.DeserializeNotificationParameters<LikeParameters>(c.Parameters).TweetAuthorFullName));

            var notificationParams = new LikeParameters()
            {
                LikingUserFullName = likeAuthorFullName,
                TweetAuthorFullName = tweetAuthorFullName,
                LikesCount = distinctLikesCount
            };

            if (notification == null)
            {
                notification = new Notification
                {
                    Parameters = _notificationMapper.SerializeNotificationParameters(notificationParams),
                    CreatedTime = DateTime.Now,
                    TweetId = tweet.Id,
                    Type = NotificationType.Like,
                    RedirectTo = string.Format(_redirectOptions.TweetUrl, tweet.Id)
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

            if (likeAuthor.Id != tweetAuthor.Id)
            {
                var tweetAuthorNotification = notifications
                    .FirstOrDefault(c => string.IsNullOrEmpty(_notificationMapper.DeserializeNotificationParameters<LikeParameters>(c.Parameters).TweetAuthorFullName));

                var tweetAuthorNotificationParams = new LikeParameters()
                {
                    LikingUserFullName = likeAuthorFullName,
                    LikesCount = distinctLikesCount
                };

                if (tweetAuthorNotification == null)
                {
                    tweetAuthorNotification = new Notification
                    {

                        Parameters = _notificationMapper.SerializeNotificationParameters(tweetAuthorNotificationParams),
                        CreatedTime = DateTime.Now,
                        TweetId = tweet.Id,
                        Type = NotificationType.Like,
                        RedirectTo = string.Format(_redirectOptions.TweetUrl, tweet.Id)
                    };

                    await _notificationRepository.AddAsync(tweetAuthorNotification);
                }
                else
                {
                    var oldNotifications = await _userNotificationRepository.GetAllByAsync(c => c.NotificationId == tweetAuthorNotification.Id);
                    await _userNotificationRepository.RemoveRangeAsync(oldNotifications);

                    tweetAuthorNotification.Parameters = _notificationMapper.SerializeNotificationParameters(tweetAuthorNotificationParams);
                    tweetAuthorNotification.CreatedTime = DateTime.Now;
                    await _notificationRepository.UpdateAsync(tweetAuthorNotification);
                }

                userNotifications.Add(new UserNotification
                {
                    NotificationId = tweetAuthorNotification.Id,
                    UserId = tweetAuthor.Id,

                });

                await _notificationService.SendNotification(tweetAuthor.Id, tweetAuthorNotification.Id);
            }

            await _userNotificationRepository.AddRangeAsync(userNotifications);

            await _notificationService.SendNotification(followers, new[] { likeAuthor.Id, tweetAuthor.Id }, notification.Id);
        }

        public async Task CreateTweetNotification(Tweet tweet, User tweetAuthor)
        {
            var tweetAuthorFullName = $"{tweetAuthor.FirstName} {tweetAuthor.LastName}";

            var notification = new Notification();

            var notificationParams = new TweetParameters()
            {
                AuthorFullName = tweetAuthorFullName,
            };

            notification.Parameters = _notificationMapper.SerializeNotificationParameters(notificationParams);
            notification.TweetId = tweet.Id;
            notification.Type = NotificationType.Tweet;
            notification.Description = tweet.Text.Truncate(30);
            notification.CreatedTime = DateTime.Now;
            notification.RedirectTo = string.Format(_redirectOptions.TweetUrl, tweet.Id);

            await _notificationRepository.AddAsync(notification);

            var followers = (await _followRepository.GetAllByAsync(c => c.FollowingId == tweetAuthor.Id)).Select(c => c.FollowerId);

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

            await _notificationService.SendNotification(followers, new[] { tweetAuthor.Id }, notification.Id);
        }
    }
}
