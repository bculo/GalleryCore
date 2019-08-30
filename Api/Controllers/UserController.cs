using Api.Models;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService service;

        public UserController(IUserService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Hello", "There" };
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public virtual async Task<IActionResult> Authenticate([FromBody] UserParams user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Username or password are empty" });
            }

            var tokenValue = await service.AuthenticateAsync(user.UserName, user.Password);
            if(tokenValue == null)
            {
                return BadRequest(new { message = "User doesnt exist" });
            }

            return Ok(new { token = tokenValue });
        }
    }
}