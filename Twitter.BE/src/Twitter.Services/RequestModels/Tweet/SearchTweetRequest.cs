namespace Twitter.Services.RequestModels.Tweet
{
    public class SearchTweetRequest : PaginationRequest
    {
        public string Query { get; set; }
    }
}
