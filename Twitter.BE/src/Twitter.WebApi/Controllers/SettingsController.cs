using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels.User;
using Twitter.Services.ResponseModels.DTOs.Settings;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IUserContext _userContext;

        public SettingsController(ISettingsService settingsService, IUserContext userContext)
        {
            _settingsService = settingsService;
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
    }
}