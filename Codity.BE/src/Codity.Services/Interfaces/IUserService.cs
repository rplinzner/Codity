using System.Threading.Tasks;
using Codity.Services.RequestModels;
using Codity.Services.RequestModels.User;
using Codity.Services.ResponseModels.DTOs.User;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Interfaces
{
    public interface IUserService
    {
        Task<IResponse<UserDTO>> GetUserAsync(int userId, int currentUserId);
        Task<IPagedResponse<BaseUserDTO>> GetUsersAsync(SearchUserRequest searchRequest, int currentUserId);
        Task<IPagedResponse<BaseUserDTO>> GetFollowersAsync(int userId, int currentUserId, PaginationRequest paginationRequest);
        Task<IPagedResponse<BaseUserDTO>> GetFollowingAsync(int userId, int currentUserId, PaginationRequest paginationRequest);
        Task<IBaseResponse> FollowUserAsync(int userId, FollowingRequest following);
        Task<IBaseResponse> UnfollowUserAsync(int userId, FollowingRequest following);
        Task<IBaseResponse> UpdateUserProfileAsync(int userId, UserProfileRequest userProfile);
    }
}
