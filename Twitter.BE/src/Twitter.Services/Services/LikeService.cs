using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Repositories.Interfaces;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels.Tweet;
using Twitter.Services.Resources;
using Twitter.Services.ResponseModels;
using Twitter.Services.ResponseModels.DTOs.Tweet;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Services
{
    public class LikeService : ILikeService
    {
        private readonly ITweetRepository _tweetRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<TweetLike> _likeRepository;
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly INotificationGeneratorService _notificationGeneratorService;
        private readonly IMapper _mapper;

        public LikeService(
            ITweetRepository tweetRepository,
            IUserRepository userRepository,
            IBaseRepository<TweetLike> likeRepository,
            IBaseRepository<Notification> notificationRepository,
            INotificationGeneratorService notificationGeneratorService,
            IMapper mapper)
        {
            _tweetRepository = tweetRepository;
            _userRepository = userRepository;
            _likeRepository = likeRepository;
            _notificationRepository = notificationRepository;
            _notificationGeneratorService = notificationGeneratorService;
            _mapper = mapper;
        }


        public async Task<IBaseResponse> CreateLikeAsync(int userId, LikeRequest like)
        {
            var response = new BaseResponse();

            var tweet = await _tweetRepository.GetByAsync(c => c.Id == like.TweetId);

            if (tweet == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.TweetNotFound
                });

                return response;
            }

            var tweetLike = await _likeRepository
                .GetByAsync(c => c.UserId == userId && c.TweetId == like.TweetId);

            if (tweetLike != null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.LikeAlreadyExists
                });

                return response;
            }

            tweetLike = new TweetLike
            {
                UserId = userId,
                TweetId = like.TweetId,
            };

            await _likeRepository.AddAsync(tweetLike);

            var likeAuthor = await _userRepository.GetAsync(userId);
            var tweetAuthor = tweet.Author;

            await _notificationGeneratorService.CreateLikeNotification(tweet, likeAuthor, tweetAuthor);

            return response;
        }

        public async Task<ICollectionResponse<LikeUserDTO>> GetLikesAsync(int tweetId)
        {
            var response = new CollectionResponse<LikeUserDTO>();

            var likes = await _likeRepository.GetAllByAsync(c => c.TweetId == tweetId, false, c => c.User);
            if (!likes.Any())
            {
                return response;
            }

            response.Models = _mapper.Map<IEnumerable<LikeUserDTO>>(likes.Select(c => c.User));

            return response;
        }

        public async Task<IBaseResponse> RemoveLikeAsync(int userId, LikeRequest like)
        {
            var response = new BaseResponse();

            var tweet = await _tweetRepository.GetByAsync(c => c.Id == like.TweetId);

            if (tweet == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.TweetNotFound
                });

                return response;
            }

            var tweetLike = await _likeRepository
                .GetByAsync(c => c.UserId == userId && c.TweetId == like.TweetId);

            if (tweetLike == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.LikeNotFound
                });

                return response;
            }

            await _likeRepository.RemoveAsync(tweetLike);

            return response;
        }
    }
}
