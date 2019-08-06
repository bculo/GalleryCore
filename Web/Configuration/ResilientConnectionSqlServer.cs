using System.Collections.Generic;

namespace Web.Configuration
{
    /// <summary>
    /// Represent class for resilient connection configuration
    /// </summary>
    internal class ResilientConnectionSqlServer
    {
        public int MaxRetryCount { get; set; }
        public int MaxRetryDelay { get; set; }
        public ICollection<int> ErrorNumbersToAdd { get; set; }
    }
}
