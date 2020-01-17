using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Codity.Services.Interfaces;
using Codity.Services.ResponseModels.DTOs.Shared;
using Codity.Services.ResponseModels.Interfaces;
using Codity.WebApi.Filters;

namespace Codity.WebApi.Controllers
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