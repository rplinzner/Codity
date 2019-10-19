using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Services.Interfaces;
using Twitter.Services.Options;
using Twitter.Shared.RequestModels.Authentication;
using Twitter.Shared.Resources;
using Twitter.Shared.ResponseModels;
using Twitter.Shared.ResponseModels.DTOs;
using Twitter.Shared.ResponseModels.Interfaces;

namespace Twitter.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSenderService _emailSenderService;
        private readonly ITokenProviderService _tokenProviderService;
        private readonly RedirectOptions _redirectOptions;
        private readonly IMapper _mapper;

        public AuthenticationService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSenderService emailSenderService,
            ITokenProviderService tokenProviderService,
            IOptions<RedirectOptions> redirectOptions,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSenderService = emailSenderService;
            _tokenProviderService = tokenProviderService;
            _redirectOptions = redirectOptions.Value;
            _mapper = mapper;
        }

        public async Task<IResponse<AuthUserDTO>> LoginAsync(LoginRequest model)
        {
            var response = new Response<AuthUserDTO>();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.IncorrectCredentials
                });

                return response;
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.EmailNotConfirmed
                });

                return response;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.IncorrectCredentials
                });

                return response;
            }

            string token = _tokenProviderService.GenerateToken(user);
            response.Model = new AuthUserDTO
            {
                Id = user.Id,
                Token = token
            };

            return response;
        }

        public async Task<IBaseResponse> RegisterAsync(RegisterRequest model)
        {
            var response = new BaseResponse();
            var user = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if (!string.IsNullOrWhiteSpace(error.Description))
                    {
                        response.AddError(new Error
                        {
                            Message = error.Description
                        });
                    }
                }

                return response;
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenVerificationUrl = $"{_redirectOptions.ConfirmEmailUrl}?id={user.Id}&token={WebUtility.UrlEncode(token)}";

            var success = await _emailSenderService.SendEmail(model.Email, "Confirm Your Email", tokenVerificationUrl);
            if (!success)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.EmailSendingError
                });
                return response;
            }

            return response;
        }

        public async Task<string> ConfirmEmailAsync(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return _redirectOptions.InvalidEmailConfirmationUrl;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return _redirectOptions.InvalidEmailConfirmationUrl;
            }

            return _redirectOptions.SuccessEmailConfirmationUrl;
        }
    }
}
