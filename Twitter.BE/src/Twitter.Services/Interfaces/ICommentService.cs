using System.Threading.Tasks;
using Twitter.Services.RequestModels;
using Twitter.Services.RequestModels.Tweet;
using Twitter.Services.ResponseModels.DTOs.Tweet;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IPagedResponse<CommentDTO>> GetCommentsAsync(int tweetId, PaginationRequest paginationRequest);
        Task<IResponse<CommentDTO>> CreateCommentAsync(int userId, CommentRequest comment);
        Task<IBaseResponse> UpdateCommentAsync(int userId, int commentId, UpdateCommentRequest comment);
        Task<IBaseResponse> RemoveCommentAsync(int userId, int commentId);
    }
}
