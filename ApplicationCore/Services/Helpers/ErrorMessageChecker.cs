using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services.Helpers
{
    public static class ErrorMessageChecker
    {
        public static bool ErrorExists(this IEnumerable<ErrorMessage> errors)
        {
            if (errors == null)
            {
                return false;
            }

            if (errors.Count() == 0)
            {
                return false;
            }

            return true;
        }
    }
}
