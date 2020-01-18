using System.ComponentModel.DataAnnotations;
using Codity.Services.Resources;

namespace Codity.Services.RequestModels.Post
{
    public class LikeRequest
    {
        [Required(ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public int PostId { get; set; }
    }
}
