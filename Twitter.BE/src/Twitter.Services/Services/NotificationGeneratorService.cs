using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Data.Model.Enums;
using Twitter.Repositories.Interfaces;
using Twitter.Services.Helpers.NotificationParameters;
using Twitter.Services.Interfaces;

namespace Twitter.Services.Services
{
    public class NotificationGeneratorService : INotificationGeneratorService
    {
        private readonly INotificationMapperService _notificationMapper;
        private readonly INotificationService _notificationService;
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly IBaseRepository<UserNotification> _userNotificationRepository;
        private readonly IBaseRepository<Follow> _followRepository;

        public NotificationGeneratorService(
            INotificationMapperService notificationMapper,
            INotificationService notificationService,
            IBaseRepository<Notification> notificationRepository,
            IBaseRepository<UserNotification> userNotificationRepository,
            IBaseRepository<Follow> followRepository)
        {
            _notificationMapper = notificationMapper;
            _notificationService = notificationService;
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
            _followRepository = followRepository;
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

                await _notificationRepository.AddAsync(tweetAuthorNotification);

                userNotifications.Add(new UserNotification
                {
                    NotificationId = tweetAuthorNotification.Id,
                    UserId = tweetAuthor.Id
                });

                var tweetAuthorNotificationDTO = _notificationMapper.MapNotification(tweetAuthorNotification);
                await _notificationService.SendNotification(tweetAuthor.Id, tweetAuthorNotificationDTO);
            }

            await _userNotificationRepository.AddRangeAsync(userNotifications);

            var notificationDTO = _notificationMapper.MapNotification(notification);

            await _notificationService.SendNotification(followers, new[] { commentAuthor.Id, tweetAuthor.Id }, notificationDTO);
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

            await _notificationRepository.AddAsync(followingNotification);

            userNotifications.Add(new UserNotification
            {
                NotificationId = followingNotification.Id,
                UserId = following.Id
            });

            var followingNotificationDTO = _notificationMapper.MapNotification(followingNotification);
            await _notificationService.SendNotification(following.Id, followingNotificationDTO);

            await _userNotificationRepository.AddRangeAsync(userNotifications);

            var notificationDTO = _notificationMapper.MapNotification(notification);

            await _notificationService.SendNotification(followers, new[] { follower.Id, following.Id }, notificationDTO);
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

            var notification = new Notification();

            var notificationParams = new LikeParameters()
            {
                LikingUserFullName = likeAuthorFullName,
                TweetAuthorFullName = tweetAuthorFullName,
                CommentsCount = distinctLikesCount
            };

            notification.Parameters = _notificationMapper.SerializeNotificationParameters(notificationParams);
            notification.TweetId = tweet.Id;
            notification.Type = NotificationType.Like;
            notification.CreatedTime = DateTime.Now;

            await _notificationRepository.AddAsync(notification);

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
                var tweetAuthorNotification = new Notification();
                var tweetAuthorNotificationParams = new LikeParameters()
                {
                    LikingUserFullName = likeAuthorFullName,
                    CommentsCount = distinctLikesCount
                };
                tweetAuthorNotification.Parameters = _notificationMapper.SerializeNotificationParameters(tweetAuthorNotificationParams);
                tweetAuthorNotification.TweetId = tweet.Id;
                tweetAuthorNotification.Type = NotificationType.Like;
                tweetAuthorNotification.CreatedTime = DateTime.Now;

                await _notificationRepository.AddAsync(tweetAuthorNotification);

                userNotifications.Add(new UserNotification
                {
                    NotificationId = tweetAuthorNotification.Id,
                    UserId = tweetAuthor.Id,

                });

                var tweetAuthorNotificationDTO = _notificationMapper.MapNotification(tweetAuthorNotification);
                await _notificationService.SendNotification(tweetAuthor.Id, tweetAuthorNotificationDTO);
            }

            await _userNotificationRepository.AddRangeAsync(userNotifications);

            var notificationDTO = _notificationMapper.MapNotification(notification);

            await _notificationService.SendNotification(followers, new[] { likeAuthor.Id, tweetAuthor.Id }, notificationDTO);
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

            var notificationDTO = _notificationMapper.MapNotification(notification);

            await _notificationService.SendNotification(followers, new[] { tweetAuthor.Id }, notificationDTO);
        }
    }
}
