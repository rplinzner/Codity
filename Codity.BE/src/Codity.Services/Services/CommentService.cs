using AutoMapper;
using System;
using System.Threading.Tasks;
using Codity.Data.Model;
using Codity.Repositories.Interfaces;
using Codity.Services.Interfaces;
using Codity.Services.RequestModels;
using Codity.Services.RequestModels.Post;
using Codity.Services.Resources;
using Codity.Services.ResponseModels;
using Codity.Services.ResponseModels.DTOs.Post;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Services
{
    public class CommentService : ICommentService
    {
        private readonly IPostRepository _postRepository;
        private readonly IBaseRepository<Comment> _commentRepository;
        private readonly INotificationGeneratorService _notificationGeneratorService;
        private readonly IMapper _mapper;

        public CommentService(
            IPostRepository postRepository,
            IBaseRepository<Comment> commentRepository,
            INotificationGeneratorService notificationGeneratorService,
            IMapper mapper)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _notificationGeneratorService = notificationGeneratorService;
            _mapper = mapper;
        }

        public async Task<IResponse<CommentDTO>> CreateCommentAsync(int userId, CommentRequest comment)
        {
            var response = new Response<CommentDTO>();

            var post = await _postRepository.GetByAsync(c => c.Id == comment.PostId);

            if (post == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.PostNotFound
                });

                return response;
            }

            var commentEntity = new Comment
            {
                AuthorId = userId,
                PostId = comment.PostId,
                Text = comment.Text,
                CreationDate = DateTime.Now
            };

            await _commentRepository.AddAsync(commentEntity);

            var result = await _commentRepository.GetByAsync(c => c.Id == commentEntity.Id, false, c => c.Author);

            response.Model = _mapper.Map<CommentDTO>(result);

            var commentAuthor = result.Author;
            var postAuthor = post.Author;

            await _notificationGeneratorService.CreateCommentNotification(post, commentAuthor, postAuthor, result.Text);

            return response;
        }

        public async Task<IPagedResponse<CommentDTO>> GetCommentsAsync(int postId, PaginationRequest paginationRequest)
        {
            var response = new PagedResponse<CommentDTO>();

            var comments = await _commentRepository.GetPagedByAsync(
                getBy: c => c.PostId == postId,
                pageNumber: paginationRequest.PageNumber,
                pageSize: paginationRequest.PageSize,
                orderBy: c => c.CreationDate,
                includes: c => c.Author);

            _mapper.Map(comments, response);

            return response;
        }

        public async Task<IBaseResponse> RemoveCommentAsync(int userId, int commentId)
        {
            var response = new BaseResponse();

            var commentEntity = await _commentRepository.GetByAsync(c => c.Id == commentId);

            if (commentEntity == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.CommentNotFound
                });

                return response;
            }

            if (commentEntity.AuthorId != userId)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.NotYourCommentRemove
                });
            }

            await _commentRepository.RemoveAsync(commentEntity);

            return response;
        }

        public async Task<IBaseResponse> UpdateCommentAsync(int userId, int commentId, UpdateCommentRequest comment)
        {
            var response = new BaseResponse();

            var commentEntity = await _commentRepository.GetByAsync(c => c.Id == commentId);

            if (commentEntity == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.CommentNotFound
                });

                return response;
            }

            if (commentEntity.AuthorId != userId)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.NotYourCommentUpdate
                });
            }

            commentEntity.Text = comment.Text;

            await _commentRepository.UpdateAsync(commentEntity);

            return response;
        }
    }
}
