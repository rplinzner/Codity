using System.Collections.Generic;

namespace Twitter.Services.ResponseModels.Interfaces
{
    public interface IBaseResponse
    {
        string Message { get; set; }
        bool IsError { get; }
        ICollection<Error> Errors { get; set; }
        void AddError(Error error);
    }
}
