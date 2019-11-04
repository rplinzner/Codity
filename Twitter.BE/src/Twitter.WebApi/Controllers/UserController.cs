using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels.User;

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

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            var response = await _userService.GetUsersAsync();

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var response = await _userService.GetUserAsync(id);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}/followers")]
        public async Task<ActionResult> GetFollowers(int id)
        {
            var response = await _userService.GetFollowersAsync(id);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}/following")]
        public async Task<ActionResult> GetFollowing(int id)
        {
            var response = await _userService.GetFollowingAsync(id);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("following")]
        public async Task<ActionResult> FollowUser([FromBody] FollowingRequest following)
        {
            int userId = _userContext.GetUserId();

            var response = await _userService.FollowUserAsync(userId, following);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("following")]
        public async Task<ActionResult> UnfollowUser([FromBody] FollowingRequest following)
        {
            int userId = _userContext.GetUserId();

            var response = await _userService.UnfollowUserAsync(userId, following);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("settings")]
        public Task<ActionResult> UpdateSettings([FromBody] SettingsRequest settings)
        {
            throw new NotImplementedException();
        }
    }
}