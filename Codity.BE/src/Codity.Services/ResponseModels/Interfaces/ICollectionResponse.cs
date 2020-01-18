using System.Collections.Generic;

namespace Codity.Services.ResponseModels.Interfaces
{
    public interface ICollectionResponse<T> : IBaseResponse where T : IResponseDTO
    {
        IEnumerable<T> Models { get; set; }
    }
}
