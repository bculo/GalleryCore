using ApplicationCore.Interfaces;
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
    [UserLogedIn]
    public class AuthenticationController : Controller
    {
        protected readonly IMapper mapper;
        protected readonly IEmailSender mailService;
        protected readonly IUrlGenerator urlService;
        protected readonly IAuthenticationService authService;

        public AuthenticationController(
            IAuthenticationService authService,
            IMapper mapper,
            IEmailSender mailService,
            IUrlGenerator urlService)
        {
            this.mailService = mailService;
            this.mapper = mapper;
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
            var (uploader, errorMessages) = await authService.RegisterUserAsync(model.UserName, model.Email, model.Password);

            if (!errorMessages.ErrorExists())
            {
                string token = await authService.CreateConfirmationTokenAsync(uploader);
                string url = urlService.CreateActivationUrl(this, uploader.UserId, token, ToString(), nameof(Confirm));
                await mailService.SendRegistrationEmailAsync(uploader.UserMail, url, token);
                return RedirectToAction(nameof(Login));
            }

            ModelStateErrorPopulator.FillWithErrors(this, errorMessages);
            return View(model);
        }
         
        [HttpGet]
        public virtual IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateModel]
        public virtual async Task<IActionResult> Login(LoginModel model)
        {
            ModelState.TryAddModelError("InvalidCredentials", "Invalid credentials.");
            return View(model);
        }

        [HttpGet]
        public virtual async Task<IActionResult> Confirm(string ident, string tok)
        {
            if (string.IsNullOrEmpty(ident) || string.IsNullOrEmpty(tok))
            {
                return BadRequest();
            }

            var messages = await authService.VerifyConfirmationTokenAsync(ident, tok);

            ModelStateErrorPopulator.FillWithErrors(this, messages);
            return View();
        }

        public override string ToString() => nameof(AuthenticationController);
    }
}