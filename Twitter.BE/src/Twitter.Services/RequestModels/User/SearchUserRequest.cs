using System.ComponentModel.DataAnnotations;
using Twitter.Services.Resources;

namespace Twitter.Services.RequestModels.User
{
    public class SearchUserRequest : PaginationRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        [MinLength(2, ErrorMessage = nameof(ErrorTranslations.SearchUserMinLength))]
        public string Query { get; set; }
    }
}
