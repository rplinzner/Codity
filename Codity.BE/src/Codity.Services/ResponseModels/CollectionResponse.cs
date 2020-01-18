using System.Collections.Generic;
using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.ResponseModels
{
    public class CollectionResponse<T> : BaseResponse, ICollectionResponse<T> where T : IResponseDTO
    {
        public IEnumerable<T> Models { get; set; }
    }
}
