using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Helpers.Service
{
    public class RequestResult<T> : Request<ServiceResult<T>>
    {
        public ServiceResult<T> BadRequest(List<string> errors)
        {
            CreateInstance(false);
            InstanceResult.Errors = SetAllErrors(errors).ToList();
            return InstanceResult;
        }

        public ServiceResult<T> BadRequest(string error)
        {
            CreateInstance(false);
            InstanceResult.Errors.Add(SetErrorMessage(0.ToString(), error));
            return InstanceResult;
        }

        public ServiceResult<T> GoodRequest(T attribute)
        {
            CreateInstance(true);
            InstanceResult.Result = attribute;
            return InstanceResult;
        }
    }
}
