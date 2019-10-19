using System.Collections.Generic;
using Twitter.Shared.ResponseModels.Interfaces;

namespace Twitter.Shared.ResponseModels
{
    public class CollectionResponse<T> : BaseResponse, ICollectionResponse<T> where T : IResponseDTO
    {
        public ICollection<T> Models { get; set; }
    }
}
