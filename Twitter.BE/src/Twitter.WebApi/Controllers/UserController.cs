using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels.User;
using Twitter.Services.ResponseModels.DTOs.User;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
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
        /// Fetch all users
        /// </summary>
        /// <returns>All Users in database</returns>
        [HttpGet]
        public async Task<ActionResult<ICollectionResponse<BaseUserDTO>>> GetUsers()
        {
            var response = await _userService.GetUsersAsync();

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
            var response = await _userService.GetUserAsync(id);

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
        /// <returns>Followers</returns>
        [HttpGet("{id}/followers")]
        public async Task<ActionResult<ICollectionResponse<BaseUserDTO>>> GetFollowers(int id)
        {
            var response = await _userService.GetFollowersAsync(id);

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
        /// <returns>Following</returns>
        [HttpGet("{id}/following")]
        public async Task<ActionResult<ICollectionResponse<BaseUserDTO>>> GetFollowing(int id)
        {
            var response = await _userService.GetFollowingAsync(id);

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
        /// NOT IMPLEMENTED
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPut("settings")]
        public Task<ActionResult> UpdateSettings([FromBody] SettingsRequest settings)
        {
            throw new NotImplementedException();
        }
    }
}