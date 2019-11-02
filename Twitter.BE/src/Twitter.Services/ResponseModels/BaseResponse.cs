using System.Collections.Generic;
using Twitter.Services.ResponseModels.Interfaces;

namespace Twitter.Services.ResponseModels
{
    public class BaseResponse : IBaseResponse
    {
        public string Message { get; set; }
        public bool IsError { get => Errors.Count > 0; }
        public ICollection<Error> Errors { get; set; } = new List<Error>();

        public void AddError(Error error)
        {
            Errors.Add(error);
        }
    }
}
