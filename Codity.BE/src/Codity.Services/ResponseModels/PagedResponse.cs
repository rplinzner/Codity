using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.ResponseModels
{
    public class PagedResponse<T> : CollectionResponse<T>, IPagedResponse<T> where T : IResponseDTO
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
