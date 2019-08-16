using ApplicationCore.Entities;
using System.Collections.Generic;

namespace ApplicationCore.Helpers.Service
{
    public class ServiceResult<T> : IServiceResponse
    {
        public T Result { get; set; }
        public bool Success { get; set; }
        public List<ErrorMessage> Errors { get; set; }
    }
}
