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
    public class GenderController : ControllerBase
    {
        private readonly IGenderService _genderService;

        public GenderController(IGenderService genderService)
        {
            _genderService = genderService;
        }

        /// <summary>
        /// Fetch all genders
        /// </summary>
        /// <returns>All genders</returns>
        [HttpGet]
        public async Task<ActionResult<ICollectionResponse<GenderDTO>>> GetGenders()
        {

            var response = await _genderService.GetGendersAsync();

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}