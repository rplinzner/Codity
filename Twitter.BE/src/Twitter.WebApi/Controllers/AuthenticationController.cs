using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels.Authentication;
using Twitter.Services.ResponseModels.DTOs.Authentication;
using Twitter.Services.ResponseModels.Interfaces;
using Twitter.WebApi.Filters;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [BaseFilter]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
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
                
                if( response.IsError)
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
    }
}