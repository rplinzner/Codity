using AutoMapper;
using System;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Repositories.Interfaces;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels;
using Twitter.Services.RequestModels.Tweet;
using Twitter.Services.Resources;
using Twitter.Services.ResponseModels;
using Twitter.Services.ResponseModels.DTOs.Tweet;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Services
{
    public class TweetService : ITweetService
    {
        private readonly ITweetRepository _tweetRepository;
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly IGithubService _githubService;
        private readonly INotificationGeneratorService _notificationGeneratorService;
        private readonly IMapper _mapper;

        public TweetService(
            ITweetRepository tweetRepository,
            IBaseRepository<Notification> notificationRepository,
            IGithubService githubService,
            INotificationGeneratorService notificationGeneratorService,
            IMapper mapper)
        {
            _tweetRepository = tweetRepository;
            _notificationRepository = notificationRepository;
            _githubService = githubService;
            _notificationGeneratorService = notificationGeneratorService;
            _mapper = mapper;
        }

        public async Task<IResponse<TweetDTO>> CreateTweetAsync(int userId, TweetRequest tweet)
        {
            var response = new Response<TweetDTO>();
            var tweetEntity = _mapper.Map<Tweet>(tweet);

            if (tweet.CodeSnippet != null && tweet.CodeSnippet.IsGist && tweet.CodeSnippet.Gist != null)
            {
                var gistResponse = await _githubService.CreateGistURL(tweet.CodeSnippet.Gist, tweet.Text, userId);
                if (gistResponse.IsError)
                {
                    response.Errors = gistResponse.Errors;
                    return response;
                }

                tweetEntity.CodeSnippet.GistURL = gistResponse.Message;
            }

            tweetEntity.AuthorId = userId;
            tweetEntity.CreationDate = DateTime.Now;

            await _tweetRepository.AddAsync(tweetEntity);

            var result = await _tweetRepository.GetByAsync(c => c.Id == tweetEntity.Id);

            response.Model = _mapper.Map<TweetDTO>(result);

            await _notificationGeneratorService.CreateTweetNotification(result, result.Author);

            return response;
        }

        public async Task<IResponse<TweetDTO>> GetTweetAsync(int tweetId)
        {
            var response = new Response<TweetDTO>();

            var tweet = await _tweetRepository.GetByAsync(c => c.Id == tweetId);

            if (tweet == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.TweetNotFound
                });

                return response;
            }

            response.Model = _mapper.Map<TweetDTO>(tweet);

            return response;
        }

        public async Task<IPagedResponse<TweetDTO>> GetTweetsAsync(PaginationRequest paginationRequest)
        {
            var response = new PagedResponse<TweetDTO>();

            var tweets = await _tweetRepository.GetPagedAsync(
                paginationRequest.PageNumber,
                paginationRequest.PageSize);

            _mapper.Map(tweets, response);

            return response;
        }

        public async Task<IBaseResponse> RemoveTweetAsync(int userId, int tweetId)
        {
            var response = new BaseResponse();

            var tweet = await _tweetRepository.GetAsync(tweetId);

            if (tweet == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.TweetNotFound
                });

                return response;
            }

            if (tweet.AuthorId != userId)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.NotYourTweetRemove
                });
            }

            var notifications = await _notificationRepository.GetAllByAsync(c => c.TweetId == tweet.Id);
            await _notificationRepository.RemoveRangeAsync(notifications);
            await _tweetRepository.RemoveAsync(tweet);

            return response;
        }

        public async Task<IBaseResponse> UpdateTweetAsync(int userId, int tweetId, UpdateTweetRequest tweet)
        {
            var response = new BaseResponse();

            var tweetEntity = await _tweetRepository.GetAsync(tweetId);

            if (tweet == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.TweetNotFound
                });

                return response;
            }

            if (tweetEntity.AuthorId != userId)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.NotYourTweetUpdate
                });
            }

            _mapper.Map(tweet, tweetEntity);

            if (tweet.CodeSnippetId.HasValue)
            {
                tweetEntity.CodeSnippet.Id = tweet.CodeSnippetId.Value;
            }

            await _tweetRepository.UpdateAsync(tweetEntity);

            return response;
        }
    }
}
