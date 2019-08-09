using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services.Helpers.ResultServices
{
    public class NoResultServiceRequest : ServiceRequest<DefaultServiceResult>
    {
        protected override void CreateInstance(bool success)
        {
            InstanceResult = new DefaultServiceResult()
            {
                Success = success ? true : false,
                Errors = new List<ErrorMessage>()
            };
        }

        public DefaultServiceResult FailedRequest(List<string> errors)
        {
            CreateInstance(false);
            InstanceResult.Errors = SetAllErrors(errors).ToList();
            return InstanceResult;
        }

        public DefaultServiceResult FailedRequest(string error)
        {
            CreateInstance(false);
            InstanceResult.Errors.Add(SetErrorMessage(0.ToString(), error));
            return InstanceResult;
        }

        public DefaultServiceResult SuccessRequest()
        {
            CreateInstance(true);
            return InstanceResult;
        }
    }
}
