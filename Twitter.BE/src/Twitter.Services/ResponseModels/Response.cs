using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels
{
    public class Response<T> : BaseResponse, IResponse<T> where T : IResponseDTO
    {
        public T Model { get; set; }
    }
}
