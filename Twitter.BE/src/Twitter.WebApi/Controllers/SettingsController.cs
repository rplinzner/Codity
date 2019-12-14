using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels.User;
using Twitter.Services.ResponseModels.DTOs.Settings;
using Twitter.Services.ResponseModels.Interfaces;
using Twitter.WebApi.Filters;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [BaseFilter]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IGithubService _githubService;
        private readonly IUserContext _userContext;
        public SettingsController(ISettingsService settingsService, IGithubService githubService, IUserContext userContext)
        {
            _settingsService = settingsService;
            _githubService = githubService;
            _userContext = userContext;
        }

        /// <summary>
        /// Get Settings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IResponse<SettingsDTO>>> GetSettings()
        {
            int userId = _userContext.GetUserId();

            var response = await _settingsService.GetSettingsAsync(userId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Update Settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<IBaseResponse>> UpdateSettings([FromBody] SettingsRequest settings)
        {
            int userId = _userContext.GetUserId();

            var response = await _settingsService.UpdateSettingsAsync(userId, settings);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Add Github Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("personal_access_token")]
        public async Task<ActionResult<IBaseResponse>> AddGithubToken([FromBody] GithubTokenRequest token)
        {
            int userId = _userContext.GetUserId();

            var response = await _githubService.AddToken(token.Token, userId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Remove Github Token
        /// </summary>
        /// <returns></returns>
        [HttpDelete("personal_access_token")]
        public async Task<ActionResult<IBaseResponse>> RemoveGithubToken()
        {
            int userId = _userContext.GetUserId();

            var response = await _githubService.RemoveToken(userId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}