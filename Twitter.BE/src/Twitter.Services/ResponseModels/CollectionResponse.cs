using System.Collections.Generic;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels
{
    public class CollectionResponse<T> : BaseResponse, ICollectionResponse<T> where T : IResponseDTO
    {
        public IEnumerable<T> Models { get; set; }
    }
}
