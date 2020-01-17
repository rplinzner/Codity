using System.Threading.Tasks;
using Codity.Data.Model;

namespace Codity.Services.Interfaces
{
    public interface INotificationGeneratorService
    {
        Task CreateCommentNotification(Post post, User commentAuthor, User postAuthor, string comment);
        Task CreateFollowNotification(User follower, User following);
        Task CreateLikeNotification(Post post, User likeAuthor, User postAuthor);
        Task CreatePostNotification(Post post, User postAuthor);
    }
}
