using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Mvc;

namespace IdentityClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Account()
        {
            ViewData["Message"] = "Your account page.";

            return View();
        }

        public async Task Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            await HttpContext.Authentication.SignOutAsync("Oidc", new AuthenticationProperties()
            {
                RedirectUri = "https://" + HttpContext.Request.Host.Value
            });
        }

        public async Task Login()
        {
            await HttpContext.Authentication.ChallengeAsync("Oidc", new AuthenticationProperties
            {
                RedirectUri = "https://" + HttpContext.Request.Host.Value
            });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
