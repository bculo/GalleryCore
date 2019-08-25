using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Extensions;
using Web.Filters;
using Web.Interfaces;
using Web.Models.Authentication;

namespace Web.Controllers
{
    [ErrorFilter]
    public class AuthenticationController : Controller, IControllerInformation
    {
        public string Name => nameof(AuthenticationController);

        protected readonly IEmailSender mailService;
        protected readonly IUrlGenerator urlService;
        protected readonly ApplicationCore.Interfaces.IAuthenticationService authService;
        protected readonly IMapper mapper;

        public AuthenticationController(
            ApplicationCore.Interfaces.IAuthenticationService authService,
            IEmailSender mailService,
            IUrlGenerator urlService,
            IMapper mapper)
        {
            this.mailService = mailService;
            this.urlService = urlService;
            this.authService = authService;
            this.mapper = mapper;
        }

        #region Registration section

        [HttpGet]
        [ValidateUserLogedIn]
        public virtual IActionResult Registration() => View();

        [HttpPost]
        [ValidateModel]
        [ValidateUserLogedIn]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Registration(RegistrationModel model)
        {
            //Service call for registering new user
            var serviceResult = await authService.RegisterUserAsync(model.UserName, model.Email, model.Password);

            if (serviceResult.Success)
            {
                //If user successfuly registered, create registration token, prepare confirmation url and send email
                IUploader uploader = serviceResult.Result;
                string token = await authService.CreateConfirmationTokenAsync(uploader);
                string url = urlService.CreateUrl(this, uploader.UserId, token, Name, nameof(Confirm));
                await mailService.SendRegistrationEmailAsync(uploader.UserMail, url, token);
                return RedirectToAction(nameof(Login));
            }

            //Display error if something went wrong
            ModelState.FillWithErrors(serviceResult.Errors);
            return View(model);
        }

        [HttpGet]
        [ValidateUserLogedIn]
        public virtual async Task<IActionResult> Confirm(string ident, string tok)
        {
            //Verify registration token
            var serviceResult = await authService.VerifyConfirmationTokenAsync(ident, tok);

            if (serviceResult.Success)
            {
                //Token successfuly confirmed
                ViewBag.Message = "Successful email confirmation";
            }
            else
            {
                //Something went wrong
                ModelState.FillWithErrors(serviceResult.Errors);
            }

            return View();
        }

        #endregion

        #region Login section

        [HttpGet]
        [ValidateUserLogedIn]
        public virtual IActionResult Login() => View();

        [HttpPost]
        [ValidateModel]
        [ValidateUserLogedIn]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Login(LoginModel model)
        {
            //Service call for user login
            var serviceResult = await authService.SignInUserAsync(model.UserName, model.Password);

            if (serviceResult.Success)
            {
                //If user loged in redirect to home page
                return RedirectToAction("Index", "Home");
            }

            //Display error if something went wrong
            ModelState.FillWithErrors(serviceResult.Errors);
            return View(model);
        }

        #endregion

        #region Password recovery section

        [HttpGet]
        [ValidateUserLogedIn]
        public virtual IActionResult PasswordRecovery() => View();

        [HttpPost]
        [ValidateModel]
        [ValidateUserLogedIn]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> PasswordRecovery(PasswordRecoveryModel model)
        {
            //Get IUploder instance from service
            IUploader uploader = await authService.GetUserByMailAsync(model.Email);

            if(uploader != null)
            {
                //If user with specified email address exist, send password recovery email
                string token = await authService.CreatePasswordRecoveryTokenAsync(uploader);
                string url = urlService.CreateUrl(this, uploader.UserId, token, Name, nameof(ConfirmPassword));
                await mailService.SendPasswordRecoveryAsync(uploader.UserMail, url, token);
                return RedirectToAction(nameof(Login));
            }

            ModelState.AddError("User", "User doesnt exist");
            return View();
        }

        [HttpGet]
        [ValidateUserLogedIn]
        public virtual IActionResult ConfirmPassword(string ident, string tok)
        {
            //Create PasswordConfirmationModel instance
            var model = new PasswordConfirmationModel
            {
                Token = tok ?? "",
                UserId = ident ?? ""
            };

            //Display view
            return View(model);
        }

        [HttpPost]
        [ValidateModel]
        [ValidateUserLogedIn]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ConfirmPassword(PasswordConfirmationModel model)
        {
            //Verify password recovery token using service
            var result = await authService.VerifyPasswordRecoveryTokenAsync(model.UserId, model.Token, model.Password);

            if (result.Success)
            {
                //Token successfuly confirmed
                ViewBag.Message = "Successful email confirmation";
            }
            else
            {
                //Something went wrong
                ModelState.FillWithErrors(result.Errors);
            }

            return View();
        }

        #endregion

        #region Logout section

        [HttpGet]
        public virtual async Task<IActionResult> Logout()
        {
            //Logout user using serice
            await authService.SignOutUser();

            //redirect user to home page
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Social providers Login / Registration

        [HttpGet]
        [ValidateUserLogedIn]
        public virtual async Task<IActionResult> LogInExternal(string externalAuthScheme)
        {
            //External login requested
            var result = await authService.GetAuthProperties(externalAuthScheme, string.Concat("Authentication/", nameof(ExternalLoginCheck)));
            if (!result.Success)
            {
                return RedirectToAction(nameof(Login));
            }

            //Map service result to AuthenticationProperties instance
            var prop = mapper.Map<AuthenticationProperties>(result.Result);

            return new ChallengeResult(externalAuthScheme, prop);
        }

        [HttpGet]
        [ValidateUserLogedIn]
        public virtual async Task<IActionResult> ExternalLoginCheck()
        {
            //Request External login
            var result = await authService.ExecuteExternalLogin();

            if (!result)
            {
                return RedirectToAction(nameof(Login));
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}