using System.Collections.Generic;

namespace Twitter.Shared.ResponseModels.Interfaces
{
    public interface IBaseResponse
    {
        string Message { get; set; }
        bool IsError { get; }
        ICollection<Error> Errors { get; set; }
        void AddError(Error error);
    }
}
