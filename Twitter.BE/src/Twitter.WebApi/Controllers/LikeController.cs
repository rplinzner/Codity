using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels.Tweet;
using Twitter.Services.ResponseModels.Interfaces;
using Twitter.WebApi.Filters;

namespace Twitter.WebApi.Controllers
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
        /// Like tweet
        /// </summary>
        /// <param name="like">Id of a tweet to like</param>
        /// <returns>Base reponse</returns>
        [HttpPost]
        public async Task<ActionResult<IBaseResponse>> LikeTweet([FromBody] LikeRequest like)
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
        /// Dislike tweet
        /// </summary>
        /// <param name="like">Id of a tweet to like</param>
        /// <returns>Base reponse</returns>
        [HttpDelete]
        public async Task<ActionResult<IBaseResponse>> DislikeTweet([FromBody] LikeRequest like)
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