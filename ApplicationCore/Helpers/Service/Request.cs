using ApplicationCore.Entities;
using System.Collections.Generic;

namespace ApplicationCore.Helpers.Service
{
    public abstract class Request<T> where T : IServiceResponse, new()
    {
        public T InstanceResult { get; set; }

        public bool IsSet
        {
            get => (InstanceResult != null) ? true : false;
        }

        /// <summary>
        /// Create instance of ErrorMessage
        /// </summary>
        /// <param name="key">name of error</param>
        /// <param name="error">error description</param>
        /// <returns>ErrorMessage instance</returns>
        protected ErrorMessage SetErrorMessage(string key, string error)
        {
            return new ErrorMessage
            {
                Key = key,
                Description = error
            };
        }

        /// <summary>
        /// Set list of ErrorMessage instances
        /// </summary>
        /// <param name="errors">list of errors as strings</param>
        /// <returns>ErrorMessage instances</returns>
        protected IEnumerable<ErrorMessage> SetAllErrors(List<string> errors)
        {
            int counter = 1;

            foreach (var error in errors)
            {
                yield return SetErrorMessage(counter.ToString(), error);
                counter++;
            }
        }

        protected virtual void CreateInstance(bool success)
        {
            InstanceResult = new T
            {
                Success = success ? true : false,
                Errors = new List<ErrorMessage>()
            };
        }
    }
}
