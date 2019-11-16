using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Data.Model.Enums;
using Twitter.Repositories.Interfaces;
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
            notification.Description = comment.Substring(0, 30);
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
                tweetAuthorNotification.Description = comment.Substring(0, 30);
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
                userNotifications.Add(new UserNotification
                {
                    NotificationId = notification.Id,
                    UserId = followerId
                });
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
            var distinctLikesCount = tweet.Comments
                .Where(c => c.AuthorId != likeAuthor.Id)
                .Select(c => c.AuthorId)
                .Distinct()
                .Count();

            var notification = await _notificationRepository
                .GetByAsync(c => c.TweetId == tweet.Id &&
                !string.IsNullOrEmpty(_notificationMapper.DeserializeNotificationParameters<LikeParameters>(c.Parameters).TweetAuthorFullName));

            var notificationParams = new LikeParameters()
            {
                LikingUserFullName = likeAuthorFullName,
                TweetAuthorFullName = tweetAuthorFullName,
                LikesCount = distinctLikesCount
            };

            notification.Parameters = _notificationMapper.SerializeNotificationParameters(notificationParams);
            notification.CreatedTime = DateTime.Now;

            if (notification == null)
            {
                notification.TweetId = tweet.Id;
                notification.Type = NotificationType.Like;
                notification.RedirectTo = string.Format(_redirectOptions.TweetUrl, tweet.Id);

                await _notificationRepository.AddAsync(notification);
            }
            else
            {
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
                var tweetAuthorNotification = await _notificationRepository
                    .GetByAsync(c => c.TweetId == tweet.Id &&
                    string.IsNullOrEmpty(_notificationMapper.DeserializeNotificationParameters<LikeParameters>(c.Parameters).TweetAuthorFullName));

                var tweetAuthorNotificationParams = new LikeParameters()
                {
                    LikingUserFullName = likeAuthorFullName,
                    LikesCount = distinctLikesCount
                };
                tweetAuthorNotification.Parameters = _notificationMapper.SerializeNotificationParameters(tweetAuthorNotificationParams);
                tweetAuthorNotification.CreatedTime = DateTime.Now;

                if (tweetAuthorNotification == null)
                {
                    tweetAuthorNotification.TweetId = tweet.Id;
                    tweetAuthorNotification.Type = NotificationType.Like;
                    tweetAuthorNotification.RedirectTo = string.Format(_redirectOptions.TweetUrl, tweet.Id);
                    await _notificationRepository.AddAsync(tweetAuthorNotification);
                }
                else
                {
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
            notification.Description = tweet.Text.Substring(0, 30);
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
