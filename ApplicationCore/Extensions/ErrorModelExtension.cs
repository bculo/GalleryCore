using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Extensions
{
    public static class ErrorModelExtension
    {
        public static Dictionary<string, string> ToDictionary(this List<ErrorMessage> errors)
        {
            if(errors == null)
            {
                return new Dictionary<string, string>();
            }

            return errors.ToDictionary(x => x.Key, x => x.Description);
        }

        public static List<string> OnlyDescription(this List<ErrorMessage> errors)
        {
            if (errors == null)
            {
                return new List<string>();
            }

            return errors.Select(item => item.Description).ToList();
        }
    }
}
