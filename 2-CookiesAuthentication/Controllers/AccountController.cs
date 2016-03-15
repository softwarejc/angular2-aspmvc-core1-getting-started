using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using Microsoft.AspNet.Authentication.Cookies;
using CookiesAuthentication.ViewModels;

namespace CookiesAuthentication.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // Create identity with our claims
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, model.Name),
                },
                // Claims schema
                CookieAuthenticationDefaults.AuthenticationScheme);

                // Convert claims into a cookie using the cookie schema, if "AutomaticAuthenticate" is true 
                // that cookie will always be read and converted into a ClaimsIdentity in every request
                await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return LocalRedirect(returnUrl ?? "/");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }
    }
}
