using ApplicationCore.Entities;
using System.Collections.Generic;

namespace ApplicationCore.Helpers.Service
{
    public interface IServiceResponse
    {
        bool Success { get; set; }
        List<ErrorMessage> Errors { get; set; }
    }
}
