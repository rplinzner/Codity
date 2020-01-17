using System.ComponentModel.DataAnnotations;
using Codity.Services.Resources;

namespace Codity.Services.RequestModels.User
{
    public class FollowingRequest
    {
        [Required(ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public int FollowingId { get; set; }
    }
}
