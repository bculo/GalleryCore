using ApplicationCore.Interfaces;
using Infrastructure.IdentityData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Extensions;
using Web.Filters;
using Web.Interfaces;
using Web.Models.Authentication;

namespace Web.Controllers
{
    [ErrorFilter]
    public class AuthenticationController : Controller
    {
        protected readonly IEmailSender mailService;
        protected readonly IUrlGenerator urlService;
        protected readonly ApplicationCore.Interfaces.IAuthenticationService authService;
        protected readonly SignInManager<GalleryUser> signInManager;

        public AuthenticationController(
            ApplicationCore.Interfaces.IAuthenticationService authService,
            IEmailSender mailService,
            IUrlGenerator urlService,
            SignInManager<GalleryUser> signInManager)
        {
            this.mailService = mailService;
            this.urlService = urlService;
            this.authService = authService;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [ValidateUserLogedIn]
        public virtual IActionResult Registration() => View();

        [HttpPost]
        [ValidateModel]
        [ValidateUserLogedIn]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Registration(RegistrationModel model)
        {
            var serviceResult = await authService.RegisterUserAsync(model.UserName, model.Email, model.Password);

            if (serviceResult.Success)
            {
                IUploader uploader = serviceResult.Result;
                string token = await authService.CreateConfirmationTokenAsync(uploader);
                string url = urlService.CreateUrl(this, uploader.UserId, token, ToString(), nameof(Confirm));
                await mailService.SendRegistrationEmailAsync(uploader.UserMail, url, token);
                return RedirectToAction(nameof(Login));
            }

            ModelState.FillWithErrors(serviceResult.Errors);
            return View(model);
        }
         
        [HttpGet]
        [ValidateUserLogedIn]
        public virtual IActionResult Login() => View();

        [HttpPost]
        [ValidateModel]
        [ValidateUserLogedIn]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Login(LoginModel model)
        {
            var serviceResult = await authService.SignInUserAsync(model.UserName, model.Password);

            if (serviceResult.Success)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.FillWithErrors(serviceResult.Errors);
            return View(model);
        }

        [HttpGet]
        [ValidateUserLogedIn]
        public virtual async Task<IActionResult> Confirm(string ident, string tok)
        {
            var serviceResult = await authService.VerifyConfirmationTokenAsync(ident, tok);

            if (serviceResult.Success)
            {
                ViewBag.Message = "Successful email confirmation";
            }
            else
            {
                ModelState.FillWithErrors(serviceResult.Errors);
            }

            return View();
        }

        [HttpGet]
        [ValidateUserLogedIn]
        public virtual IActionResult PasswordRecovery() => View();

        [HttpPost]
        [ValidateModel]
        [ValidateUserLogedIn]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> PasswordRecovery(PasswordRecoveryModel model)
        {
            IUploader uploader = await authService.GetUserByMailAsync(model.Email);

            if(uploader != null)
            {
                string token = await authService.CreatePasswordRecoveryTokenAsync(uploader);
                string url = urlService.CreateUrl(this, uploader.UserId, token, ToString(), nameof(ConfirmPassword));
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
            var model = new PasswordConfirmationModel
            {
                Token = tok,
                UserId = ident
            };

            return View(model);
        }

        [HttpPost]
        [ValidateModel]
        [ValidateUserLogedIn]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> ConfirmPassword(PasswordConfirmationModel model)
        {
            var result = await authService.VerifyPasswordRecoveryTokenAsync(model.UserId, model.Token, model.Password);

            if (result.Success)
            {
                ViewBag.Message = "Successful email confirmation";
            }
            else
            {
                ModelState.FillWithErrors(result.Errors);
            }

            return View();
        }

        [HttpGet]
        public virtual async Task<IActionResult> Logout()
        {
            await authService.SignOutUser();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [ValidateUserLogedIn]
        public virtual async Task<IActionResult> LogInExternal(string externalAuthScheme)
        {
            var result = await authService.GetAuthProperties(externalAuthScheme, string.Concat("Authentication/", nameof(ExternalLoginCheck)));

            if (!result.Success)
            {
                return RedirectToAction(nameof(Login));
            }

            return new ChallengeResult(externalAuthScheme, result.Result as AuthenticationProperties);
        }

        [HttpGet]
        [ValidateUserLogedIn]
        public async Task<IActionResult> ExternalLoginCheck()
        {
            if(await authService.ValidteExternalAuthentication())
            {
                ModelState.AddError("ERROR", "Error with social login");
                return View();
            }


            return View();
        }

        public override string ToString() => nameof(AuthenticationController);
    }
}