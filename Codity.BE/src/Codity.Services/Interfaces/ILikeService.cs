using System.Threading.Tasks;
using Codity.Services.RequestModels;
using Codity.Services.RequestModels.Post;
using Codity.Services.ResponseModels.DTOs.Post;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Interfaces
{
    public interface ILikeService
    {
        Task<IPagedResponse<LikeUserDTO>> GetLikesAsync(int postId, PaginationRequest paginationRequest);
        Task<IBaseResponse> CreateLikeAsync(int userId, LikeRequest like);
        Task<IBaseResponse> RemoveLikeAsync(int userId, LikeRequest like);
    }
}
