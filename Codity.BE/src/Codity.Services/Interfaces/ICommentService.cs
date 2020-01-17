using System.Threading.Tasks;
using Codity.Services.RequestModels;
using Codity.Services.RequestModels.Post;
using Codity.Services.ResponseModels.DTOs.Post;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IPagedResponse<CommentDTO>> GetCommentsAsync(int postId, PaginationRequest paginationRequest);
        Task<IResponse<CommentDTO>> CreateCommentAsync(int userId, CommentRequest comment);
        Task<IBaseResponse> UpdateCommentAsync(int userId, int commentId, UpdateCommentRequest comment);
        Task<IBaseResponse> RemoveCommentAsync(int userId, int commentId);
    }
}
