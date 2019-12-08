using System.Threading.Tasks;
using Twitter.Services.RequestModels;
using Twitter.Services.RequestModels.Tweet;
using Twitter.Services.ResponseModels.DTOs.Tweet;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Interfaces
{
    public interface ITweetService
    {
        Task<IResponse<TweetDTO>> GetTweetAsync(int tweetId);
        Task<IPagedResponse<TweetDTO>> GetTweetsAsync(PaginationRequest paginationRequest);
        Task<IResponse<TweetDTO>> CreateTweetAsync(int userId, TweetRequest tweet);
        Task<IBaseResponse> UpdateTweetAsync(int userId, int tweetId, UpdateTweetRequest tweet);
        Task<IBaseResponse> RemoveTweetAsync(int userId, int tweetId);
    }
}
