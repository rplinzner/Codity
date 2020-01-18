using System.ComponentModel.DataAnnotations;
using Codity.Services.Resources;

namespace Codity.Services.RequestModels.Post
{
    public class GistRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public string Description { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public string FileName { get; set; }
    }
}
