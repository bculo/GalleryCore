using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services.Helpers.ResultServices
{
    public class ResultServiceRequest<T> : ServiceRequest<ServiceResult<T>>
    {
        protected override void CreateInstance(bool success)
        {
            InstanceResult = new ServiceResult<T>
            {
                Success = success ? true : false,
                Errors = new List<ErrorMessage>()
            };
        }

        public ServiceResult<T> FailedRequest(List<string> errors)
        {
            CreateInstance(false);
            InstanceResult.Errors = SetAllErrors(errors).ToList();
            return InstanceResult;
        }

        public ServiceResult<T> FailedRequest(string error)
        {
            CreateInstance(false);
            InstanceResult.Errors.Add(SetErrorMessage(0.ToString(), error));
            return InstanceResult;
        }

        public ServiceResult<T> SuccessRequest(T attribute)
        {
            CreateInstance(true);
            InstanceResult.Result = attribute;
            return InstanceResult;
        }
    }
}
