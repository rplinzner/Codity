using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using Codity.Services.Interfaces;

namespace Codity.Services.Services
{
    public class UserContext : IUserContext
    {
        private readonly HttpContext _httpContext;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public int GetUserId()
        {
            var id = _httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(id, out int result))
            {
                throw new UnauthorizedAccessException();
            }

            return result;
        }
    }
}
