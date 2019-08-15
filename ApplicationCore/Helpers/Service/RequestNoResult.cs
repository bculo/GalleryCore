using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Helpers.Service
{
    public class RequestNoResult : Request<ServiceNoResult>
    {
        public ServiceNoResult BadRequest(List<string> errors)
        {
            CreateInstance(false);
            InstanceResult.Errors = SetAllErrors(errors).ToList();
            return InstanceResult;
        }

        public ServiceNoResult BadRequest(string error)
        {
            CreateInstance(false);
            InstanceResult.Errors.Add(SetErrorMessage(0.ToString(), error));
            return InstanceResult;
        }

        public ServiceNoResult GoodRequest()
        {
            CreateInstance(true);
            return InstanceResult;
        }
    }
}
