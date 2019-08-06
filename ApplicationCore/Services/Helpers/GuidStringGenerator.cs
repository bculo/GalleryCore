using ApplicationCore.Interfaces;
using System;

namespace ApplicationCore.Services.Helpers
{
    public class GuidStringGenerator : IUniqueStringGenerator
    {
        /// <summary>
        /// Generate unique string with Guid class
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateUniqueString() => Guid.NewGuid().ToString();
    }
}
