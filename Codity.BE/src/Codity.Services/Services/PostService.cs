using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using Codity.Data.Model;
using Codity.Repositories.Interfaces;
using Codity.Services.Interfaces;
using Codity.Services.RequestModels;
using Codity.Services.Resources;
using Codity.Services.ResponseModels;
using Codity.Services.ResponseModels.Interfaces;
using Codity.Services.RequestModels.Post;
using Codity.Services.ResponseModels.DTOs.Post;

namespace Codity.Services.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly IBaseRepository<PostLike> _postLikeRepository;
        private readonly IBaseRepository<Follow> _followRepository;
        private readonly IGithubService _githubService;
        private readonly INotificationGeneratorService _notificationGeneratorService;
        private readonly IMapper _mapper;

        public PostService(
            IPostRepository postRepository,
            IBaseRepository<Notification> notificationRepository,
            IBaseRepository<PostLike> postLikeRepository,
            IBaseRepository<Follow> followRepository,
            IGithubService githubService,
            INotificationGeneratorService notificationGeneratorService,
            IMapper mapper)
        {
            _postRepository = postRepository;
            _notificationRepository = notificationRepository;
            _postLikeRepository = postLikeRepository;
            _followRepository = followRepository;
            _githubService = githubService;
            _notificationGeneratorService = notificationGeneratorService;
            _mapper = mapper;
        }

        public async Task<IResponse<PostDTO>> CreatePostAsync(int userId, PostRequest post)
        {
            var response = new Response<PostDTO>();
            var postEntity = _mapper.Map<Post>(post);

            if (post.CodeSnippet != null && post.CodeSnippet.IsGist && post.CodeSnippet.Gist != null)
            {
                var gistResponse = await _githubService.CreateGistURL(post.CodeSnippet.Gist, post.CodeSnippet.Text, userId);
                if (gistResponse.IsError)
                {
                    response.Errors = gistResponse.Errors;
                    return response;
                }

                postEntity.CodeSnippet.GistURL = gistResponse.Message;
            }

            postEntity.AuthorId = userId;
            postEntity.CreationDate = DateTime.Now;

            await _postRepository.AddAsync(postEntity);

            var result = await _postRepository.GetByAsync(c => c.Id == postEntity.Id);

            response.Model = _mapper.Map<PostDTO>(result);

            await _notificationGeneratorService.CreatePostNotification(result, result.Author);

            return response;
        }

        public async Task<IResponse<PostDTO>> GetPostAsync(int postId, int currentUserId)
        {
            var response = new Response<PostDTO>();

            var post = await _postRepository.GetByAsync(c => c.Id == postId);

            if (post == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.PostNotFound
                });

                return response;
            }
            var postDTO = _mapper.Map<PostDTO>(post);
            postDTO.IsLiked = await _postLikeRepository.ExistAsync(c => c.UserId == currentUserId && c.PostId == postDTO.Id);

            response.Model = postDTO;

            return response;
        }

        public async Task<IPagedResponse<PostDTO>> GetPostsAsync(PaginationRequest paginationRequest, int currentUserId)
        {
            var response = new PagedResponse<PostDTO>();

            var following = (await _followRepository.GetAllByAsync(c => c.FollowerId == currentUserId)).Select(c => c.FollowingId);

            var posts = await _postRepository.GetPagedByAsync(
                c => following.Contains(c.AuthorId),
                paginationRequest.PageNumber,
                paginationRequest.PageSize);

            _mapper.Map(posts, response);

            var postIds = posts.Select(c => c.Id);
            var likes = await _postLikeRepository.GetAllByAsync(c => postIds.Contains(c.PostId) && c.UserId == currentUserId);
            foreach (var model in response.Models)
            {
                model.IsLiked = likes.Any(c => c.PostId == model.Id);
            }

            return response;
        }

        public async Task<IBaseResponse> RemovePostAsync(int userId, int postId)
        {
            var response = new BaseResponse();

            var post = await _postRepository.GetAsync(postId);

            if (post == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.PostNotFound
                });

                return response;
            }

            if (post.AuthorId != userId)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.NotYourPostRemove
                });
            }

            var notifications = await _notificationRepository.GetAllByAsync(c => c.PostId == post.Id);
            await _notificationRepository.RemoveRangeAsync(notifications);
            await _postRepository.RemoveAsync(post);

            return response;
        }

        public async Task<IBaseResponse> UpdatePostAsync(int userId, int postId, UpdatePostRequest post)
        {
            var response = new BaseResponse();

            var postEntity = await _postRepository.GetAsync(postId);

            if (post == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.PostNotFound
                });

                return response;
            }

            if (postEntity.AuthorId != userId)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.NotYourPostUpdate
                });
            }

            _mapper.Map(post, postEntity);

            if (post.CodeSnippetId.HasValue)
            {
                postEntity.CodeSnippet.Id = post.CodeSnippetId.Value;
            }

            await _postRepository.UpdateAsync(postEntity);

            return response;
        }
    }
}
