using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class ServiceResult<T> : DefaultServiceResult
    {
        public T Result { get; set; }
    }
}
