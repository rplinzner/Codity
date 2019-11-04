using System.ComponentModel.DataAnnotations;

namespace Twitter.Services.RequestModels.Authentication
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
