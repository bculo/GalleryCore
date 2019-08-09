using ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Web.Extensions
{
    public static class ModelStateExtension
    {
        public static void FillWithErrors(this ModelStateDictionary modelstate, List<string> errors)
        {
            if (errors == null)
            {
                return;
            }

            int counter = 1;
            foreach (var error in errors)
            {
                modelstate.AddError(counter.ToString(), error);
                counter++;
            }
        }

        public static void FillWithErrors(this ModelStateDictionary modelstate, Dictionary<string, string> errors)
        {
            if (errors == null)
            {
                return;
            }

            foreach (var error in errors)
            {
                modelstate.AddError(error.Key, error.Value);
            }
        }

        public static void FillWithErrors(this ModelStateDictionary modelstate, List<ErrorMessage> errors)
        {
            if (errors == null)
            {
                return;
            }

            foreach (var error in errors)
            {
                modelstate.AddError(error.Key, error.Description);
            }
        }

        public static void AddError(this ModelStateDictionary modelstate, string key, string value)
        {
            modelstate.TryAddModelError(key, value);
        }
    }
}
