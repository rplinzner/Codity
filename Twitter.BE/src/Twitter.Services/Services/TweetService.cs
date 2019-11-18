using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    public class TweetService : ITweetService
    {
        private readonly ITweetRepository _tweetRepository;
        private readonly IBaseRepository<Notification> _notificationRepository;
        private readonly INotificationGeneratorService _notificationGeneratorService;
        private readonly IMapper _mapper;

        public TweetService(
            ITweetRepository tweetRepository,
            IBaseRepository<Notification> notificationRepository,
            INotificationGeneratorService notificationGeneratorService,
            IMapper mapper)
        {
            _tweetRepository = tweetRepository;
            _notificationRepository = notificationRepository;
            _notificationGeneratorService = notificationGeneratorService;
            _mapper = mapper;
        }

        public async Task<IResponse<TweetDTO>> CreateTweetAsync(int userId, TweetRequest tweet)
        {
            var response = new Response<TweetDTO>();

            var tweetEntity = _mapper.Map<Tweet>(tweet);

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
                    Message = ErrorTranslations.UserNotFound
                });

                return response;
            }

            response.Model = _mapper.Map<TweetDTO>(tweet);

            return response;
        }

        public async Task<ICollectionResponse<TweetDTO>> GetTweetsAsync()
        {
            var response = new CollectionResponse<TweetDTO>();

            var tweets = await _tweetRepository.GetAllAsync();

            response.Models = _mapper.Map<IEnumerable<TweetDTO>>(tweets);

            return response;
        }

        public async Task<IPagedResponse<TweetDTO>> GetTweetsAsync(SearchTweetRequest searchRequest)
        {
            var response = new PagedResponse<TweetDTO>();

            var searchExpression = CreateSearchExpression(searchRequest);
            var tweets = await _tweetRepository.GetAllByAsync(
                searchExpression,
                searchRequest.PageNumber,
                searchRequest.PageSize);

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
                    Message = ErrorTranslations.UserNotFound
                });

                return response;
            }

            if (tweet.AuthorId != userId)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.UserNotFound
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
                    Message = ErrorTranslations.UserNotFound
                });

                return response;
            }

            if (tweetEntity.AuthorId != userId)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.UserNotFound
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

        private Expression<Func<Tweet, bool>> CreateSearchExpression(SearchTweetRequest searchRequest)
        {
            var query = searchRequest.Query.ToLower();

            Expression<Func<Tweet, bool>> searchExpression = c => true;

            if (!string.IsNullOrEmpty(searchRequest.Query))
            {
                searchExpression = c => c.Text.ToLower().Contains(query) ||
                  (c.CodeSnippet != null && c.CodeSnippet.Text.ToLower().Contains(query)) ||
                  c.Author.FirstName.ToLower().Contains(query) ||
                  c.Author.LastName.ToLower().Contains(query);
            }

            return searchExpression;
        }
    }
}
