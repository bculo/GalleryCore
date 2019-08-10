using System;

namespace ApplicationCore.Helpers.Generator
{
    public class GuidStringGenerator : IUniqueStringGenerator
    {
        public virtual string GenerateUniqueString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
