using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Repositories.Interfaces;
using Twitter.Services.Helpers;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels.Tweet;
using Twitter.Services.Resources;
using Twitter.Services.ResponseModels;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.Services
{
    public class GithubService : IGithubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBaseRepository<Settings> _settingsRepository;

        public GithubService(IHttpClientFactory httpClientFactory, IBaseRepository<Settings> settingsRepository)
        {
            _httpClientFactory = httpClientFactory;
            _settingsRepository = settingsRepository;
        }

        public async Task<IBaseResponse> CreateGistURL(GistRequest gist, string text, int currentUserId)
        {
            var response = new BaseResponse();

            var settings = await _settingsRepository.GetByAsync(c => c.UserId == currentUserId);

            if (settings == null || string.IsNullOrEmpty(settings.GithubToken))
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.GithubTokenNotFound
                });
                return response;
            }

            var validationResponse = await ValidateToken(settings.GithubToken);

            if (validationResponse.IsError)
            {
                return validationResponse;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, "/gists");
            request.Headers.Add("Authorization", $"token {settings.GithubToken}");

            var files = new Dictionary<string, object>();
            files.Add(gist.FileName, new { content = text });

            var gistModel = new GistModel
            {
                Description = gist.Description,
                Public = true,
                Files = files
            };

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var json = JsonConvert.SerializeObject(gistModel, serializerSettings);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            request.Content = content;
            var client = _httpClientFactory.CreateClient("github");

            var clientReponse = await client.SendAsync(request);
            if (clientReponse.StatusCode != HttpStatusCode.Created)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.GithubGistError
                });

                return response;
            }

            var responseData = await clientReponse.Content.ReadAsStringAsync();
            response.Message = JObject.Parse(responseData).Value<string>("html_url");

            return response;
        }

        public async Task<IBaseResponse> AddToken(string token, int currentUserId)
        {
            var response = await ValidateToken(token);

            if (response.IsError)
            {
                return response;
            }

            var settings = await _settingsRepository.GetByAsync(c => c.UserId == currentUserId);

            if (settings == null)
            {
                settings = new Settings
                {
                    IsDarkTheme = false,
                    LanguageId = 1,
                    UserId = currentUserId,
                    GithubToken = token
                };

                await _settingsRepository.AddAsync(settings);
            }
            else
            {
                settings.GithubToken = token;

                await _settingsRepository.UpdateAsync(settings);
            }

            return response;
        }

        public async Task<IBaseResponse> RemoveToken(int currentUserId)
        {
            var response = new BaseResponse();

            var settings = await _settingsRepository.GetByAsync(c => c.UserId == currentUserId);

            if (settings == null || string.IsNullOrEmpty(settings.GithubToken))
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.GithubTokenNotFound
                });

                return response;
            }

            settings.GithubToken = null;
            await _settingsRepository.UpdateAsync(settings);

            return response;
        }

        private async Task<IBaseResponse> ValidateToken(string token)
        {
            var response = new BaseResponse();

            var request = new HttpRequestMessage(HttpMethod.Get, "");
            request.Headers.Add("Authorization", $"token {token}");

            var client = _httpClientFactory.CreateClient("github");
            var clientReponse = await client.SendAsync(request);

            if (clientReponse.StatusCode != HttpStatusCode.OK)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.InvalidGithubToken
                });

                return response;
            }

            var scopes = clientReponse.Headers.GetValues("X-OAuth-Scopes");

            if (!scopes.Contains("gist"))
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.GithubTokenMissingGists
                });

                return response;
            }

            return response;
        }
    }
}
