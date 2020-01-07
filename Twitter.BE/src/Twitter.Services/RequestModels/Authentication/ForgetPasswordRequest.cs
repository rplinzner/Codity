using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Twitter.Services.Resources;

namespace Twitter.Services.RequestModels.Authentication
{
    public class ForgetPasswordRequest
    {
        [EmailAddress(ErrorMessage = nameof(ErrorTranslations.EmailError))]
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public string Email { get; set; }
    }
}
