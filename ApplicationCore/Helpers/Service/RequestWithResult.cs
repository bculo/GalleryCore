using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Helpers.Service
{
    public class RequestWithResult<T> : Request<ServiceResult<T>>
    {
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
