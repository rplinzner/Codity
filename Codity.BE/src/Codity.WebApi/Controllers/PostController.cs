using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Codity.Services.Interfaces;
using Codity.Services.RequestModels;
using Codity.Services.ResponseModels.Interfaces;
using Codity.WebApi.Filters;
using Codity.Services.ResponseModels.DTOs.Post;
using Codity.Services.RequestModels.Post;

namespace Codity.WebApi.Controllers
{
    [Route("api/[controller]")]
    [BaseFilter]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;
        private readonly IUserContext _userContext;

        public PostController(IPostService postService, ICommentService commentService, ILikeService likeService, IUserContext userContext)
        {
            _postService = postService;
            _commentService = commentService;
            _likeService = likeService;
            _userContext = userContext;
        }

        /// <summary>
        /// Get paginated posts
        /// </summary>
        /// <param name="paginationRequest">Pagination parameters</param>
        /// <returns>Posts</returns>
        [HttpGet]
        public async Task<ActionResult<IPagedResponse<PostDTO>>> GetPosts([FromQuery] PaginationRequest paginationRequest)
        {
            int userId = _userContext.GetUserId();

            var response = await _postService.GetPostsAsync(paginationRequest, userId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Fetch post by id
        /// </summary>
        /// <param name="postId">Id of a post</param>
        /// <returns>Post with specified id</returns>
        [HttpGet("{postId}")]
        public async Task<ActionResult<IResponse<PostDTO>>> GetPost(int postId)
        {
            int userId = _userContext.GetUserId();

            var response = await _postService.GetPostAsync(postId, userId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Fetch likes for a post with specified id
        /// </summary>
        /// <param name="postId">Id of a post</param>
        /// <param name="paginationRequest">Pagination parameters</param>
        /// <returns>Likes</returns>
        [HttpGet("{postId}/like")]
        public async Task<ActionResult<IPagedResponse<LikeUserDTO>>> GetLikes(int postId, [FromQuery] PaginationRequest paginationRequest)
        {
            var response = await _likeService.GetLikesAsync(postId, paginationRequest);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Fetch comments for a post with specified id
        /// </summary>
        /// <param name="postId">Id of a post</param>
        /// <param name="paginationRequest">Pagination parameters</param>
        /// <returns>Comments</returns>
        [HttpGet("{postId}/comment")]
        public async Task<ActionResult<IPagedResponse<CommentDTO>>> GetComments(int postId, [FromQuery] PaginationRequest paginationRequest)
        {
            var response = await _commentService.GetCommentsAsync(postId, paginationRequest);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Add post
        /// </summary>
        /// <param name="post">Post</param>
        /// <returns>Base reponse</returns>
        [HttpPost]
        public async Task<ActionResult<IResponse<PostDTO>>> CreatePost([FromBody] PostRequest post)
        {
            int userId = _userContext.GetUserId();

            var response = await _postService.CreatePostAsync(userId, post);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Remove post with specified Id
        /// </summary>
        /// <param name="postId">Id of a post to remove</param>
        /// <returns>Base reponse</returns>
        [HttpDelete("{postId}")]
        public async Task<ActionResult<IBaseResponse>> RemovePost(int postId)
        {
            int userId = _userContext.GetUserId();

            var response = await _postService.RemovePostAsync(userId, postId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Update Post
        /// </summary>
        /// <param name="postId">Id of a post</param>
        /// <param name="post">Updated post</param>
        /// <returns></returns>
        [HttpPut("{postId}")]
        public async Task<ActionResult<IBaseResponse>> UpdatePost(int postId, [FromBody] UpdatePostRequest post)
        {
            int userId = _userContext.GetUserId();

            var response = await _postService.UpdatePostAsync(userId, postId, post);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}