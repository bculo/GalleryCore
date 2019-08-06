using ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Web.Services
{
    public static class ModelStateErrorPopulator
    {
        public static void FillWithErrors(ControllerBase controller, IEnumerable<ErrorMessage> errors)
        {
            foreach (var error in errors)
            {
                controller.ModelState.TryAddModelError(error.Key, error.Description);
            }
        }
    }
}
