using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Repositories.Interfaces;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels.User;
using Twitter.Services.Resources;
using Twitter.Services.ResponseModels;
using Twitter.Services.ResponseModels.DTOs.Tweet;
using Twitter.Services.ResponseModels.DTOs.User;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<Follow> _followRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationGeneratorService _notificationGeneratorService;
        private readonly IMapper _mapper;

        public UserService(
            IBaseRepository<Follow> followRepository,
            IUserRepository userRepository,
            INotificationGeneratorService notificationGeneratorService,
            IMapper mapper)
        {
            _followRepository = followRepository;
            _userRepository = userRepository;
            _notificationGeneratorService = notificationGeneratorService;
            _mapper = mapper;
        }

        public async Task<IResponse<UserDTO>> GetUserAsync(int userId, int currentUserId)
        {
            var response = new Response<UserDTO>();

            var user = await _userRepository.GetByAsync(
                c => c.Id == userId,
                false,
                c => c.Gender,
                c => c.Followers,
                c => c.Following,
                c => c.Tweets);

            if (user == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.UserNotFound
                });

                return response;
            }

            var userDTO = _mapper.Map<UserDTO>(user);

            userDTO.LatestTweets = _mapper.Map<IEnumerable<TweetDTO>>(user.Tweets.Take(10));
            userDTO.IsFollowing = await _followRepository.ExistAsync(c => c.FollowerId == currentUserId && c.FollowingId == userId);

            response.Model = userDTO;

            return response;
        }

        public async Task<IBaseResponse> UnfollowUserAsync(int userId, FollowingRequest following)
        {
            var response = new BaseResponse();

            var follow = await _followRepository
                .GetByAsync(c => c.FollowerId == userId && c.FollowingId == following.FollowingId);

            if (follow == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.FollowNotFound
                });

                return response;
            }

            await _followRepository.RemoveAsync(follow);

            return response;
        }

        public async Task<IBaseResponse> FollowUserAsync(int userId, FollowingRequest following)
        {
            var response = new BaseResponse();
            var followerUser = await _userRepository.GetAsync(userId);
            var followingUser = await _userRepository.GetAsync(following.FollowingId);

            if (followerUser == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.UserNotFound
                });

                return response;
            }

            if (followingUser == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.UserNotFound
                });

                return response;
            }

            var follow = await _followRepository
                .GetByAsync(c => c.FollowerId == userId && c.FollowingId == following.FollowingId);

            if (follow != null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.FollowAlreadyExists
                });

                return response;
            }

            follow = new Follow
            {
                FollowerId = userId,
                FollowingId = following.FollowingId
            };

            await _followRepository.AddAsync(follow);

            await _notificationGeneratorService.CreateFollowNotification(followerUser, followingUser);

            return response;
        }

        public async Task<ICollectionResponse<BaseUserDTO>> GetFollowersAsync(int userId, int currentUserId)
        {
            var response = new CollectionResponse<BaseUserDTO>();

            var follows = await _followRepository
                .GetAllByAsync(c => c.FollowingId == userId, false, c => c.Follower);

            if (!follows.Any())
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.FollowersNotFound
                });

                return response;
            }

            var followers = follows.Select(c => c.Follower);
            response.Models = _mapper.Map<IEnumerable<BaseUserDTO>>(followers);

            var userIds = followers.Select(c => c.Id);
            var following = await _followRepository.GetAllByAsync(c => userIds.Contains(c.FollowingId) && c.FollowerId == currentUserId);

            foreach (var model in response.Models)
            {
                model.IsFollowing = following.Any(c => c.FollowingId == model.Id);
            }

            return response;
        }

        public async Task<ICollectionResponse<BaseUserDTO>> GetFollowingAsync(int userId, int currentUserId)
        {
            var response = new CollectionResponse<BaseUserDTO>();

            var follows = await _followRepository
                .GetAllByAsync(c => c.FollowerId == userId, false, c => c.Following);

            if (!follows.Any())
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.FollowingNotFound
                });

                return response;
            }

            var following = follows.Select(c => c.Following);
            response.Models = _mapper.Map<IEnumerable<BaseUserDTO>>(following);

            var userIds = following.Select(c => c.Id);
            var currentUsersFollowing = await _followRepository.GetAllByAsync(c => userIds.Contains(c.FollowingId) && c.FollowerId == currentUserId);

            foreach (var model in response.Models)
            {
                model.IsFollowing = currentUsersFollowing.Any(c => c.FollowingId == model.Id);
            }

            return response;
        }

        public async Task<IPagedResponse<BaseUserDTO>> GetUsersAsync(SearchUserRequest searchRequest, int currentUserId)
        {
            var response = new PagedResponse<BaseUserDTO>();

            var searchExpression = CreateSearchExpression(searchRequest);
            var users = await _userRepository.SearchAsync(
                searchRequest.Query,
                searchRequest.PageNumber,
                searchRequest.PageSize);

            _mapper.Map(users, response);

            var userIds = users.Select(c => c.Id);
            var following = await _followRepository.GetAllByAsync(c => userIds.Contains(c.FollowingId) && c.FollowerId == currentUserId);
            foreach (var model in response.Models)
            {
                model.IsFollowing = following.Any(c => c.FollowingId == model.Id);
            }

            return response;
        }

        public async Task<IBaseResponse> UpdateUserProfileAsync(int userId, UserProfileRequest userProfile)
        {
            var response = new BaseResponse();

            var user = await _userRepository.GetAsync(userId);

            _mapper.Map(userProfile, user);

            await _userRepository.UpdateAsync(user);

            return response;
        }

        private Expression<Func<User, bool>> CreateSearchExpression(SearchUserRequest searchRequest)
        {

            Expression<Func<User, bool>> searchExpression = c => true;

            if (!string.IsNullOrEmpty(searchRequest.Query))
            {
                var query = searchRequest.Query.ToLower();
                searchExpression = c => EF.Functions.Contains(c.FirstName, query) || EF.Functions.Contains(c.LastName, query) || EF.Functions.Contains(c.AboutMe, query);
            }

            return searchExpression;
        }
    }
}
