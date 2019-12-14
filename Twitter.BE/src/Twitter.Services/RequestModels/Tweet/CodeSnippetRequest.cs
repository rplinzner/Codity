using System.ComponentModel.DataAnnotations;
using Twitter.Services.Resources;

namespace Twitter.Services.RequestModels.Tweet
{
    public class CodeSnippetRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public string Text { get; set; }
        [Required(ErrorMessage = nameof(ErrorTranslations.RequiredError))]
        public int ProgrammingLanguageId { get; set; }

        public bool IsGist { get; set; }
        public GistRequest Gist { get; set; }
    }
}
