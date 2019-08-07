using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services.Helpers.ResultServices
{
    public class NoResultServiceRequest : ServiceRequest<DefaultServiceResult>
    {
        protected override DefaultServiceResult CreateInstance(bool success)
        {
            return new DefaultServiceResult()
            {
                Success = success ? true : false,
                Errors = new List<ErrorMessage>()
            };
        }

        public DefaultServiceResult FailedRequest(List<string> errors)
        {
            DefaultServiceResult instance = CreateInstance(false);
            instance.Errors = SetAllErrors(errors).ToList();
            return instance;
        }

        public DefaultServiceResult FailedRequest(string error)
        {
            DefaultServiceResult instance = CreateInstance(false);
            instance.Errors.Add(SetErrorMessage(0.ToString(), error));
            return instance;
        }

        public DefaultServiceResult SuccessRequest()
        {
            DefaultServiceResult instance = CreateInstance(true);
            return instance;
        }
    }
}
