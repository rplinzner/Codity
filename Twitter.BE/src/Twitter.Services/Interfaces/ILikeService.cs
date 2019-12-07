using System.Threading.Tasks;
using Twitter.Services.RequestModels.Tweet;
using Twitter.Services.ResponseModels.DTOs.Tweet;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Interfaces
{
    public interface ILikeService
    {
        Task<ICollectionResponse<LikeUserDTO>> GetLikesAsync(int tweetId);
        Task<IBaseResponse> CreateLikeAsync(int userId, LikeRequest like);
        Task<IBaseResponse> RemoveLikeAsync(int userId, LikeRequest like);
    }
}
