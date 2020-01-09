using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Services.Interfaces;
using Twitter.Services.ResponseModels.DTOs.Shared;
using Twitter.Services.ResponseModels.Interfaces;
using Twitter.WebApi.Filters;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [BaseFilter]
    [Authorize]
    public class ProgrammingLanguageController : ControllerBase
    {
        private readonly IProgrammingLanguageService _programmingLanguageService;

        public ProgrammingLanguageController(IProgrammingLanguageService programmingLanguageService)
        {
            _programmingLanguageService = programmingLanguageService;
        }

        /// <summary>
        /// Fetch all programming languages
        /// </summary>
        /// <returns>All programming languages</returns>
        [HttpGet]
        public async Task<ActionResult<ICollectionResponse<ProgrammingLanguageDTO>>> GetGenders()
        {
            var response = await _programmingLanguageService.GetProgrammingLanguagesAsync();

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}