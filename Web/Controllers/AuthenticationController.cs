﻿using ApplicationCore.Interfaces;
using ApplicationCore.Services.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Filters;
using Web.Interfaces;
using Web.Models.Authentication;
using Web.Services;

namespace Web.Controllers
{
    [ErrorFilter]
    [ValidateUserLogedIn]
    public class AuthenticationController : Controller
    {
        protected readonly IEmailSender mailService;
        protected readonly IUrlGenerator urlService;
        protected readonly IAuthenticationService authService;

        public AuthenticationController(
            IAuthenticationService authService,
            IEmailSender mailService,
            IUrlGenerator urlService)
        {
            this.mailService = mailService;
            this.urlService = urlService;
            this.authService = authService;
        }

        [HttpGet]
        public virtual IActionResult Registration() => View();

        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Registration(RegistrationModel model)
        {
            var serviceResult = await authService.RegisterUserAsync(model.UserName, model.Email, model.Password);

            if (serviceResult.Success)
            {
                IUploader uploader = serviceResult.Result;
                string token = await authService.CreateConfirmationTokenAsync(uploader);
                string url = urlService.CreateActivationUrl(this, uploader.UserId, token, ToString(), nameof(Confirm));
                await mailService.SendRegistrationEmailAsync(uploader.UserMail, url, token);
                return RedirectToAction(nameof(Login));
            }

            ModelStateErrorPopulator.FillWithErrors(this, serviceResult.Errors);
            return View(model);
        }
         
        [HttpGet]
        public virtual IActionResult Login() => View();

        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Login(LoginModel model)
        {
            ModelState.TryAddModelError("InvalidCredentials", "Invalid credentials.");
            return View(model);
        }

        [HttpGet]
        public virtual async Task<IActionResult> Confirm(string ident, string tok)
        {
            var serviceResult = await authService.VerifyConfirmationTokenAsync(ident, tok);
            ModelStateErrorPopulator.FillWithErrors(this, serviceResult.Errors);
            return View();
        }

        [HttpGet]
        public virtual IActionResult PasswordRecovery() => View();

        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> PasswordRecovery(PasswordRecoveryModel model)
        {
            IUploader uploader = await authService.GetUserByMailAsync(model.Email);

            if(uploader != null)
            {
                string token = await authService.CreatePasswordRecoveryTokenAsync(uploader);
                string url = urlService.CreatePasswordRecoveryUrl(this, uploader.UserId, token, ToString(), nameof(ConfirmPassword));
                await mailService.SendRegistrationEmailAsync(uploader.UserMail, url, token);
                return RedirectToAction(nameof(Login));
            }

            ModelState.TryAddModelError("User", "User doesnt exist");
            return View();
        }

        [HttpGet]
        public virtual IActionResult ConfirmPassword(string ident, string to) => View();

        public override string ToString() => nameof(AuthenticationController);
    }
}