using System.Collections.Generic;

namespace Twitter.Shared.ResponseModels.Interfaces
{
    public interface ICollectionResponse<T> : IBaseResponse where T : IResponseDTO
    {
        ICollection<T> Models { get; set; }
    }
}
