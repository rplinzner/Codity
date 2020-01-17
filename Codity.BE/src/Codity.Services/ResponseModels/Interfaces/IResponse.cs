namespace Codity.Services.ResponseModels.Interfaces
{
    public interface IResponse<T> : IBaseResponse where T : IResponseDTO
    {
        T Model { get; set; }
    }
}
