using System;
using System.Linq;
using System.Net.Http;
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

        [Authorize]
        public async Task<IActionResult> CallApi()
        {
            var accessToken = User.Claims.SingleOrDefault(claim => claim.Type == "access_token");

            if (accessToken == null)
            {
                return new BadRequestResult();
            }

            HttpClient client = new HttpClient();

            // The Resource API will validate this access token
            client.SetBearerToken(accessToken.Value);
            client.BaseAddress = new Uri(Constants.ResourceApi);

            HttpResponseMessage response = await client.GetAsync("api/Tickets/Read");

            if (!response.IsSuccessStatusCode)
            {
                return new BadRequestObjectResult("Error connecting with the API");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            return new ObjectResult(responseContent);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
