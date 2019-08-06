using ApplicationCore.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Web.Interfaces;

namespace Web.Services
{
    public class UrlGenerator : IUrlGenerator
    {
        private string Delimiter { get; } = "/";
        private string CurrentRoute { get; set; }
        private string CallbackURL { get; set; }
        private string DestinationRoute { get; set; }

        public virtual string CreateActivationUrl(ControllerBase controller, string userId, string token, string controllerName, string actionName)
        {
            CurrentRoute = GetCurrentRoute(controller);
            DestinationRoute = SetFinalRoute(controllerName, actionName);

            object obj = new
            {
                ident = userId,
                tok = token
            };

            CallbackURL = SetCallbackUrl(controller, obj, DestinationRoute);
            return Regex.Replace(CallbackURL, CurrentRoute, DestinationRoute);
        }

        public virtual string CreatePasswordRecoveryUrl(ControllerBase controller, Uploader user, string controllerName, string actionName)
        {
            CurrentRoute = GetCurrentRoute(controller);
            DestinationRoute = SetFinalRoute(controllerName, actionName);

            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            //byte[] key = Encoding.UTF8.GetBytes(user.HelpId);
            byte[] key = Encoding.UTF8.GetBytes(string.Empty);
            string token = Convert.ToBase64String(time.Concat(key).ToArray());

            object obj = new
            {
                token = ""
            };

            CallbackURL = SetCallbackUrl(controller, obj, DestinationRoute);
            return Regex.Replace(CallbackURL, CurrentRoute, DestinationRoute);
        }

        protected virtual string GetUserTokenAs64String(Uploader user)
        {
            //byte[] key = Encoding.UTF8.GetBytes(user.HelpId);
            byte[] tokenArray = Encoding.UTF8.GetBytes(string.Empty);
            return Convert.ToBase64String(tokenArray);
        }

        protected virtual string GetCurrentRoute(ControllerBase controller)
        {
            var controllerName = controller.ControllerContext.RouteData.Values["controller"].ToString();
            var actionName = controller.ControllerContext.RouteData.Values["action"].ToString();
            return JoinStringElements(Delimiter, controllerName, Delimiter, actionName);
        }

        protected virtual string SetFinalRoute(string controllerName, string actionName)
        {
            controllerName = controllerName.Replace("Controller", "");
            return JoinStringElements(Delimiter, controllerName, Delimiter, actionName); 
        }

        protected virtual string JoinStringElements(params string[] param)
        {
            string finalRoute = string.Empty;

            foreach(var item in param)
            {
                finalRoute = string.Concat(finalRoute, item);
            }

            return finalRoute;
        }

        protected virtual string SetCallbackUrl(ControllerBase controller, object obj, string destinationRoute)
        {
            return controller.Url.Page(
                pageName: destinationRoute,
                pageHandler: null,
                values: obj,
                protocol: controller.Request.Scheme);
        }
    }
}
