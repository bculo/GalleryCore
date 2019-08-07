using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class DefaultServiceResult
    {
        public bool Success { get; set; }
        public List<ErrorMessage> Errors { get; set; }
    }
}
