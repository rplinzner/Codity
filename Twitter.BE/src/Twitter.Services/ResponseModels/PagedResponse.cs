using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels
{
    public class PagedResponse<T> : CollectionResponse<T>, IPagedResponse<T> where T : IResponseDTO
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
