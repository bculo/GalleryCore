using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Helpers.Service
{
    public class RequestNoResult : Request<ServiceNoResult>
    {
        protected override void CreateInstance(bool success)
        {
            InstanceResult = new ServiceNoResult()
            {
                Success = success ? true : false,
                Errors = new List<ErrorMessage>()
            };
        }

        public ServiceNoResult FailedRequest(List<string> errors)
        {
            CreateInstance(false);
            InstanceResult.Errors = SetAllErrors(errors).ToList();
            return InstanceResult;
        }

        public ServiceNoResult FailedRequest(string error)
        {
            CreateInstance(false);
            InstanceResult.Errors.Add(SetErrorMessage(0.ToString(), error));
            return InstanceResult;
        }

        public ServiceNoResult SuccessRequest()
        {
            CreateInstance(true);
            return InstanceResult;
        }
    }
}
