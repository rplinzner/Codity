namespace Twitter.Services.RequestModels.Tweet
{
    public class TweetRequest
    {
        public string Text { get; set; }
        public CodeSnippetRequest CodeSnippet { get; set; }
    }
}
