using AutoMapper;
using System.Collections.Generic;
using System.Linq;
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
    public class LikeService : ILikeService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<PostLike> _likeRepository;
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly INotificationGeneratorService _notificationGeneratorService;
        private readonly IMapper _mapper;

        public LikeService(
            IPostRepository postRepository,
            IUserRepository userRepository,
            IBaseRepository<PostLike> likeRepository,
            IBaseRepository<Notification> notificationRepository,
            INotificationGeneratorService notificationGeneratorService,
            IMapper mapper)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _likeRepository = likeRepository;
            _notificationRepository = notificationRepository;
            _notificationGeneratorService = notificationGeneratorService;
            _mapper = mapper;
        }


        public async Task<IBaseResponse> CreateLikeAsync(int userId, LikeRequest like)
        {
            var response = new BaseResponse();

            var post = await _postRepository.GetByAsync(c => c.Id == like.PostId);

            if (post == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.PostNotFound
                });

                return response;
            }

            var postLike = await _likeRepository
                .GetByAsync(c => c.UserId == userId && c.PostId == like.PostId);

            if (postLike != null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.LikeAlreadyExists
                });

                return response;
            }

            postLike = new PostLike
            {
                UserId = userId,
                PostId = like.PostId,
            };

            await _likeRepository.AddAsync(postLike);

            var likeAuthor = await _userRepository.GetAsync(userId);
            var postAuthor = post.Author;

            await _notificationGeneratorService.CreateLikeNotification(post, likeAuthor, postAuthor);

            return response;
        }

        public async Task<IPagedResponse<LikeUserDTO>> GetLikesAsync(int postId, PaginationRequest paginationRequest)
        {
            var response = new PagedResponse<LikeUserDTO>();

            var likes = await _likeRepository.GetPagedByAsync(
               c => c.PostId == postId,
               paginationRequest.PageNumber,
               paginationRequest.PageSize,
               false,
               c => c.User);

            if (!likes.Any())
            {
                return response;
            }

            _mapper.Map(likes, response);

            var likeUsers = likes.Select(c => c.User);
            response.Models = _mapper.Map<IEnumerable<LikeUserDTO>>(likeUsers);

            return response;
        }

        public async Task<IBaseResponse> RemoveLikeAsync(int userId, LikeRequest like)
        {
            var response = new BaseResponse();

            var post = await _postRepository.GetByAsync(c => c.Id == like.PostId);

            if (post == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.PostNotFound
                });

                return response;
            }

            var postLike = await _likeRepository
                .GetByAsync(c => c.UserId == userId && c.PostId == like.PostId);

            if (postLike == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.LikeNotFound
                });

                return response;
            }

            await _likeRepository.RemoveAsync(postLike);

            return response;
        }
    }
}
