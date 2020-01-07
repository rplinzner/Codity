using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels;
using Twitter.Services.RequestModels.Tweet;
using Twitter.Services.ResponseModels.DTOs.Tweet;
using Twitter.Services.ResponseModels.Interfaces;
using Twitter.WebApi.Filters;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [BaseFilter]
    [Authorize]
    public class TweetController : ControllerBase
    {
        private readonly ITweetService _tweetService;
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;
        private readonly IUserContext _userContext;

        public TweetController(ITweetService tweetService, ICommentService commentService, ILikeService likeService, IUserContext userContext)
        {
            _tweetService = tweetService;
            _commentService = commentService;
            _likeService = likeService;
            _userContext = userContext;
        }

        /// <summary>
        /// Get paginated tweets
        /// </summary>
        /// <param name="paginationRequest">Pagination parameters</param>
        /// <returns>Tweets</returns>
        [HttpGet]
        public async Task<ActionResult<IPagedResponse<TweetDTO>>> GetTweets([FromQuery] PaginationRequest paginationRequest)
        {
            var response = await _tweetService.GetTweetsAsync(paginationRequest);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Fetch tweet by id
        /// </summary>
        /// <param name="tweetId">Id of a tweet</param>
        /// <returns>Tweet with specified id</returns>
        [HttpGet("{tweetId}")]
        public async Task<ActionResult<IResponse<TweetDTO>>> GetTweet(int tweetId)
        {
            var response = await _tweetService.GetTweetAsync(tweetId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Fetch likes for a tweet with specified id
        /// </summary>
        /// <param name="tweetId">Id of a tweet</param>
        /// <returns>Likes</returns>
        [HttpGet("{tweetId}/like")]
        public async Task<ActionResult<ICollectionResponse<LikeUserDTO>>> GetLikes(int tweetId)
        {
            var response = await _likeService.GetLikesAsync(tweetId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Fetch comments for a tweet with specified id
        /// </summary>
        /// <param name="tweetId">Id of a tweet</param>
        /// <param name="paginationRequest">Pagination parameters</param>
        /// <returns>Comments</returns>
        [HttpGet("{tweetId}/comment")]
        public async Task<ActionResult<IPagedResponse<CommentDTO>>> GetComments(int tweetId, [FromQuery] PaginationRequest paginationRequest)
        {
            var response = await _commentService.GetCommentsAsync(tweetId, paginationRequest);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Add tweet
        /// </summary>
        /// <param name="tweet">Tweet</param>
        /// <returns>Base reponse</returns>
        [HttpPost]
        public async Task<ActionResult<IResponse<TweetDTO>>> CreateTweet([FromBody] TweetRequest tweet)
        {
            int userId = _userContext.GetUserId();

            var response = await _tweetService.CreateTweetAsync(userId, tweet);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Remove tweet with specified Id
        /// </summary>
        /// <param name="tweetId">Id of a tweet to remove</param>
        /// <returns>Base reponse</returns>
        [HttpDelete("{tweetId}")]
        public async Task<ActionResult<IBaseResponse>> RemoveTweet(int tweetId)
        {
            int userId = _userContext.GetUserId();

            var response = await _tweetService.RemoveTweetAsync(userId, tweetId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Update Tweet
        /// </summary>
        /// <param name="tweetId">Id of a tweet</param>
        /// <param name="tweet">Updated tweet</param>
        /// <returns></returns>
        [HttpPut("{tweetId}")]
        public async Task<ActionResult<IBaseResponse>> UpdateTweet(int tweetId, [FromBody] UpdateTweetRequest tweet)
        {
            int userId = _userContext.GetUserId();

            var response = await _tweetService.UpdateTweetAsync(userId, tweetId, tweet);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}