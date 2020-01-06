using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels;
using Twitter.Services.RequestModels.User;
using Twitter.Services.ResponseModels.DTOs.User;
using Twitter.Services.ResponseModels.Interfaces;
using Twitter.WebApi.Filters;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [BaseFilter]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserContext _userContext;

        public UserController(IUserService userService, IUserContext userContext)
        {
            _userService = userService;
            _userContext = userContext;
        }

        /// <summary>
        /// Search users
        /// </summary>
        /// <returns>Users</returns>
        [BaseFilter]
        [HttpGet("search")]
        public async Task<ActionResult<IPagedResponse<BaseUserDTO>>> SearchUsers([FromQuery] SearchUserRequest searchUserRequest)
        {
            int userId = _userContext.GetUserId();

            var response = await _userService.GetUsersAsync(searchUserRequest, userId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Fetch user by id
        /// </summary>
        /// <param name="id">Id of an user</param>
        /// <returns>User with specified id</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IResponse<UserDTO>>> GetUser(int id)
        {
            int userId = _userContext.GetUserId();

            var response = await _userService.GetUserAsync(id, userId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Fetch followers for an user with specified id
        /// </summary>
        /// <param name="id">Id of an user</param>
        /// <param name="paginationRequest">Pagination parameters</param>
        /// <returns>Followers</returns>
        [HttpGet("{id}/followers")]
        public async Task<ActionResult<IPagedResponse<BaseUserDTO>>> GetFollowers(int id, [FromQuery] PaginationRequest paginationRequest)
        {
            int userId = _userContext.GetUserId();

            var response = await _userService.GetFollowersAsync(id, userId, paginationRequest);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Fetch following for an user with specified id
        /// </summary>
        /// <param name="id">If of an user</param>
        /// <param name="paginationRequest">Pagination parameters</param>
        /// <returns>Following</returns>
        [HttpGet("{id}/following")]
        public async Task<ActionResult<IPagedResponse<BaseUserDTO>>> GetFollowing(int id, [FromQuery] PaginationRequest paginationRequest)
        {
            int userId = _userContext.GetUserId();

            var response = await _userService.GetFollowingAsync(id, userId, paginationRequest);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Follow user with specified Id
        /// </summary>
        /// <param name="following">Id of an user to follow</param>
        /// <returns>Base reponse</returns>
        [HttpPost("following")]
        public async Task<ActionResult<IBaseResponse>> FollowUser([FromBody] FollowingRequest following)
        {
            int userId = _userContext.GetUserId();

            var response = await _userService.FollowUserAsync(userId, following);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Unfollow user with specified Id
        /// </summary>
        /// <param name="following">Id of an user to unfollow</param>
        /// <returns>Base reponse</returns>
        [HttpDelete("following")]
        public async Task<ActionResult<IBaseResponse>> UnfollowUser([FromBody] FollowingRequest following)
        {
            int userId = _userContext.GetUserId();

            var response = await _userService.UnfollowUserAsync(userId, following);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpPut("profile")]
        public async Task<ActionResult<IBaseResponse>> UpdateUserProfile([FromBody] UserProfileRequest userProfile)
        {
            int userId = _userContext.GetUserId();

            var response = await _userService.UpdateUserProfileAsync(userId, userProfile);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}