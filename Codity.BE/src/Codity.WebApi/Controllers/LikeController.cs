using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Codity.Services.Interfaces;
using Codity.Services.ResponseModels.Interfaces;
using Codity.WebApi.Filters;
using Codity.Services.RequestModels.Post;

namespace Codity.WebApi.Controllers
{
    [Route("api/[controller]")]
    [BaseFilter]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;
        private readonly IUserContext _userContext;

        public LikeController(ILikeService likeService, IUserContext userContext)
        {
            _likeService = likeService;
            _userContext = userContext;
        }

        /// <summary>
        /// Like post
        /// </summary>
        /// <param name="like">Id of a post to like</param>
        /// <returns>Base reponse</returns>
        [HttpPost]
        public async Task<ActionResult<IBaseResponse>> LikePost([FromBody] LikeRequest like)
        {
            int userId = _userContext.GetUserId();

            var response = await _likeService.CreateLikeAsync(userId, like);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Dislike post
        /// </summary>
        /// <param name="like">Id of a post to like</param>
        /// <returns>Base reponse</returns>
        [HttpDelete]
        public async Task<ActionResult<IBaseResponse>> DislikePost([FromBody] LikeRequest like)
        {
            int userId = _userContext.GetUserId();

            var response = await _likeService.RemoveLikeAsync(userId, like);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}