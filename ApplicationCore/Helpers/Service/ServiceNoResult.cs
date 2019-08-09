using ApplicationCore.Entities;
using System.Collections.Generic;

namespace ApplicationCore.Helpers.Service
{
    public class ServiceNoResult
    {
        public bool Success { get; set; }
        public List<ErrorMessage> Errors { get; set; }
    }
}
