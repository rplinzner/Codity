namespace Twitter.Services.RequestModels.User
{
    public class SearchUserRequest : PaginationRequest
    {
        public string Query { get; set; }
    }
}
