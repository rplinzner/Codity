﻿using System.Threading.Tasks;
using Twitter.Services.RequestModels;
using Twitter.Services.RequestModels.User;
using Twitter.Services.ResponseModels.DTOs.User;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Interfaces
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
