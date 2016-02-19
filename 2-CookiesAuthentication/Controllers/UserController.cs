using Microsoft.AspNet.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using Microsoft.AspNet.Authentication.Cookies;

namespace Security_ASPNetCore1_Angular2.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Public()
        {
            return new ObjectResult("this is public...");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Secret()
        {
            return new ObjectResult("this is secret!!");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Info()
        {
            return new ObjectResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // todo write a simple login form asking for the name...
            // ...

            // Create identity with our claims
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "Juan Carlos"),

            },
            // Claims schema
            CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            // After login is there is a return url redirect...
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return new LocalRedirectResult(returnUrl);
            }

            // ... if not show an info message
            return new ObjectResult("logged IN");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new ObjectResult("logged OUT");
        }
    }
}
