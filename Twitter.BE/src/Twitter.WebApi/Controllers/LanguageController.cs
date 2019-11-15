using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Services.Interfaces;
using Twitter.Services.ResponseModels.DTOs.Shared;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        /// <summary>
        /// Fetch all languages
        /// </summary>
        /// <returns>All languages</returns>
        [HttpGet]
        public async Task<ActionResult<ICollectionResponse<LanguageDTO>>> GetLanguages()
        {

            var response = await _languageService.GetLanguagesAsync();

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
