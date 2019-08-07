using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore.Services.Helpers.ResultServices
{
    public class ResultServiceRequest<T> : ServiceRequest<ServiceResult<T>>
    {
        protected override ServiceResult<T> CreateInstance(bool success)
        {
            return new ServiceResult<T>
            {
                Success = success ? true : false,
                Errors = new List<ErrorMessage>()
            };
        }

        public ServiceResult<T> FailedRequest(List<string> errors)
        {
            ServiceResult<T> instance = CreateInstance(false);
            instance.Errors = SetAllErrors(errors).ToList();
            return instance;
        }

        public ServiceResult<T> FailedRequest(string error)
        {
            ServiceResult<T> instance = CreateInstance(false);
            instance.Errors.Add(SetErrorMessage(0.ToString(), error));
            return instance;
        }

        public ServiceResult<T> SuccessRequest(T attribute)
        {
            ServiceResult<T> instance = CreateInstance(true);
            instance.Result = attribute;
            return instance;
        }
    }
}
