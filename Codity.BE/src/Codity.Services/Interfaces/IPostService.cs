using System.Threading.Tasks;
using Codity.Services.RequestModels;
using Codity.Services.RequestModels.Post;
using Codity.Services.ResponseModels.DTOs.Post;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Interfaces
{
    public interface IPostService
    {
        Task<IResponse<PostDTO>> GetPostAsync(int postId, int currentUserId);
        Task<IPagedResponse<PostDTO>> GetPostsAsync(PaginationRequest paginationRequest, int currentUserId);
        Task<IResponse<PostDTO>> CreatePostAsync(int userId, PostRequest post);
        Task<IBaseResponse> UpdatePostAsync(int userId, int postId, UpdatePostRequest post);
        Task<IBaseResponse> RemovePostAsync(int userId, int postId);
    }
}
