using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Twitter.Data.Model;

namespace Twitter.Services.Interfaces
{
    public interface INotificationGeneratorService
    {
        Task CreateCommentNotification(Tweet tweet, User commentAuthor, User tweetAuthor, string comment);
        Task CreateFollowNotification(User follower, User following);
        Task CreateLikeNotification(Tweet tweet, User likeAuthor, User tweetAuthor);
        Task CreateTweetNotification(Tweet tweet, User tweetAuthor);
    }
}
