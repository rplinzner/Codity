﻿using System.Threading.Tasks;
using Twitter.Services.RequestModels.User;
using Twitter.Services.ResponseModels.DTOs.User;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Interfaces
{
    public interface IUserService
    {
        Task<IResponse<UserDTO>> GetUserAsync(int userId);
        Task<ICollectionResponse<BaseUserDTO>> GetUsersAsync();
        Task<ICollectionResponse<BaseUserDTO>> GetFollowersAsync(int userId);
        Task<ICollectionResponse<BaseUserDTO>> GetFollowingAsync(int userId);
        Task<IBaseResponse> FollowUserAsync(int userId, FollowingRequest following);
        Task<IBaseResponse> UnfollowUserAsync(int userId, FollowingRequest following);
    }
}