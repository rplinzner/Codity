using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.ResponseModels
{
    public class Response<T> : BaseResponse, IResponse<T> where T : IResponseDTO
    {
        public T Model { get; set; }
    }
}
