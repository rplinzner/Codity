using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels.Tweet;
using Twitter.Services.ResponseModels.DTOs.Tweet;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly ITweetService _tweetService;
        private readonly IUserContext _userContext;

        public TweetController(ITweetService tweetService, IUserContext userContext)
        {
            _tweetService = tweetService;
            _userContext = userContext;
        }


        /// <summary>
        /// Search tweets
        /// </summary>
        /// <param name="searchTweetRequest">Search parameters</param>
        /// <returns>Tweets</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IPagedResponse<TweetDTO>>> SearchTweets([FromQuery] SearchTweetRequest searchTweetRequest)
        {
            var response = await _tweetService.GetTweetsAsync(searchTweetRequest);

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
        /// NOT IMPLEMENTED YET
        /// Fetch likes for a tweet with specified id
        /// </summary>
        /// <param name="tweetId">Id of a tweet</param>
        /// <returns>Likes</returns>
        [HttpGet("{tweetId}/like")]
        public async Task<ActionResult<IBaseResponse>> GetLikes(int tweetId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// NOT IMPLEMENTED YET
        /// Fetch comments for a tweet with specified id
        /// </summary>
        /// <param name="tweetId">Id of a tweet</param>
        /// <returns>Comments</returns>
        [HttpGet("{tweetId}/comment")]
        public async Task<ActionResult<IBaseResponse>> GetComments(int tweetId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Add tweet
        /// </summary>
        /// <param name="tweet">Tweet</param>
        /// <returns>Base reponse</returns>
        [HttpPost]
        public async Task<ActionResult<IBaseResponse>> CreateTweet([FromBody] TweetRequest tweet)
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