using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Twitter.Services.Interfaces;
using Twitter.Services.RequestModels.Authentication;

namespace Twitter.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var response = await _authenticationService.LoginAsync(model);
                return Ok(response);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

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
                return Ok(response);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var response = await _authenticationService.RegisterAsync(model);
                return Ok(response);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}