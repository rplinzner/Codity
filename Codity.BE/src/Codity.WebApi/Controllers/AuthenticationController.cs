using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Codity.Services.Interfaces;
using Codity.Services.RequestModels.Authentication;
using Codity.Services.ResponseModels.DTOs.Authentication;
using Codity.Services.ResponseModels.Interfaces;
using Codity.WebApi.Filters;

namespace Codity.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [BaseFilter]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserContext _userContext;

        public AuthenticationController(IAuthenticationService authenticationService, IUserContext userContext)
        {
            _authenticationService = authenticationService;
            _userContext = userContext;
        }

        /// <summary>
        /// Allows user to login to application using email and password
        /// </summary>
        /// <param name="model">email and password</param>
        /// <returns>Id of an user and token</returns>
        [HttpPost]
        public async Task<ActionResult<IResponse<AuthUserDTO>>> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var response = await _authenticationService.LoginAsync(model);

                if (response.IsError)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Email confirmation
        /// </summary>
        /// <param name="id">Id of an user</param>
        /// <param name="token">Confirmation token</param>
        /// <returns>Redirect or BadRequest when exception is thrown</returns>
        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string id, string token)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var response = await _authenticationService.ConfirmEmailAsync(id, token);
                return Redirect(response);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Allows user to register to application via email and password
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Base response (sends email to user)</returns>
        [HttpPost]
        public async Task<ActionResult<IBaseResponse>> Register([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var response = await _authenticationService.RegisterAsync(model);

                if (response.IsError)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Sends to user reset password token
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Base response</returns>
        [HttpPost]
        public async Task<ActionResult<IBaseResponse>> ForgetPassword([FromBody] ForgetPasswordRequest model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var response = await _authenticationService.ForgetPasswordAsync(model);

                if (response.IsError)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Allows user to reset password
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Base response</returns>
        [HttpPut]
        public async Task<ActionResult<IBaseResponse>> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var response = await _authenticationService.ResetPasswordAsync(model);

                if (response.IsError)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Allows user to change password
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Base response</returns>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<IBaseResponse>> ChangePassword([FromBody] ChangePasswordRequest model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var userId = _userContext.GetUserId();

                var response = await _authenticationService.ChangePasswordAsync(userId, model);

                if (response.IsError)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}