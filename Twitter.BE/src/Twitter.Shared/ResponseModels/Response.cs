using Twitter.Shared.ResponseModels.Interfaces;

namespace Twitter.Shared.ResponseModels
{
    public class Response<T> : BaseResponse, IResponse<T> where T : IResponseDTO
    {
        public T Model { get; set; }
    }

}
