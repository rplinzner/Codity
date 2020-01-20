using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Net;
using System.Threading.Tasks;
using Codity.Data.Model;
using Codity.Repositories.Interfaces;
using Codity.Services.Interfaces;
using Codity.Services.Options;
using Codity.Services.RequestModels.Authentication;
using Codity.Services.Resources;
using Codity.Services.ResponseModels;
using Codity.Services.ResponseModels.DTOs.Authentication;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IBaseRepository<Language> _languageRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSenderService _emailSenderService;
        private readonly ITokenProviderService _tokenProviderService;
        private readonly RedirectOptions _redirectOptions;
        private readonly IMapper _mapper;

        public AuthenticationService(
            IBaseRepository<Language> languageRepository,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSenderService emailSenderService,
            ITokenProviderService tokenProviderService,
            IOptions<RedirectOptions> redirectOptions,
            IMapper mapper)
        {
            _languageRepository = languageRepository;
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

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
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

            var language = await _languageRepository.GetByAsync(c => c.Code == model.LanguageCode);
            if (language == null)
            {
                response.AddError(new Error
                {
                    Message = string.Format(ErrorTranslations.LanguageCodeNotFound, model.LanguageCode)
                });

                return response;
            }

            user.Settings = new Settings
            {
                IsDarkTheme = model.IsDarkTheme,
                LanguageId = language.Id
            };

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
            var tokenVerificationUrl = string.Format(_redirectOptions.ConfirmEmailUrl, user.Id, WebUtility.UrlEncode(token));
            var message = string.Format(NotificationTranslations.ConfirmEmail, user.FirstName, tokenVerificationUrl);
            var emailTitle = NotificationTranslations.ConfirmEmailTitle;

            var success = await _emailSenderService.SendEmail(model.Email, emailTitle, message);
            if (!success)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.EmailSendingError
                });
            }

            return response;
        }

        public async Task<IBaseResponse> ChangePasswordAsync(int userId, ChangePasswordRequest model)
        {
            var response = new BaseResponse();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.UserNotFound
                });

                return response;
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
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

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var tokenResetPasswordUrl = string.Format(_redirectOptions.ResetPasswordUrl, WebUtility.UrlEncode(token));
            var message = string.Format(NotificationTranslations.ChangePasswordEmail, user.FirstName, tokenResetPasswordUrl);
            var emailTitle = NotificationTranslations.ChangePasswordEmailTitle;

            var success = await _emailSenderService.SendEmail(user.Email, emailTitle, message);
            if (!success)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.EmailSendingError
                });
            }

            return response;
        }

        public async Task<IBaseResponse> ForgetPasswordAsync(ForgetPasswordRequest model)
        {
            var response = new BaseResponse();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.UserNotFound
                });

                return response;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var tokenResetPasswordUrl = string.Format(_redirectOptions.ResetPasswordUrl, WebUtility.UrlEncode(token));
            var message = string.Format(NotificationTranslations.ForgetPasswordEmail, user.FirstName, tokenResetPasswordUrl);
            var emailTitle = NotificationTranslations.ForgetPasswordEmailTitle;

            var success = await _emailSenderService.SendEmail(model.Email, emailTitle, message);
            if (!success)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.EmailSendingError
                });
            }

            return response;
        }

        public async Task<IBaseResponse> ResetPasswordAsync(ResetPasswordRequest model)
        {
            var response = new BaseResponse();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                response.AddError(new Error
                {
                    Message = ErrorTranslations.UserNotFound
                });

                return response;
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
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
