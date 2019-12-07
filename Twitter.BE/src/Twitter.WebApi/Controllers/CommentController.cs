using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels.Tweet;
using Twitter.Services.ResponseModels.DTOs.Tweet;
using Twitter.Services.ResponseModels.Interfaces;
using Twitter.WebApi.Filters;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]")]
    [BaseFilter]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IUserContext _userContext;

        public CommentController(ICommentService commentService, IUserContext userContext)
        {
            _commentService = commentService;
            _userContext = userContext;
        }

        /// <summary>
        /// Create comment
        /// </summary>
        /// <param name="comment">Comment model</param>
        /// <returns>Base reponse</returns>
        [HttpPost]
        public async Task<ActionResult<IResponse<CommentDTO>>> CreateComment([FromBody] CommentRequest comment)
        {
            int userId = _userContext.GetUserId();

            var response = await _commentService.CreateCommentAsync(userId, comment);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Edit comment
        /// </summary>
        /// <param name="commentId">Updated comment Id</param>
        /// <param name="comment">Updated comment model</param>
        /// <returns>Base reponse</returns>
        [HttpPut("{commentId}")]
        public async Task<ActionResult<IBaseResponse>> UpdateComment(int commentId, [FromBody] UpdateCommentRequest comment)
        {
            int userId = _userContext.GetUserId();

            var response = await _commentService.UpdateCommentAsync(userId, commentId, comment);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Remove comment
        /// </summary>
        /// <param name="commentId">Id of a comment to remove</param>
        /// <returns>Base reponse</returns>
        [HttpDelete("{commentId}")]
        public async Task<ActionResult<IBaseResponse>> RemoveComment(int commentId)
        {
            int userId = _userContext.GetUserId();

            var response = await _commentService.RemoveCommentAsync(userId, commentId);

            if (response.IsError)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}