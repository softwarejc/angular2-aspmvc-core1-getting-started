using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Linq;

namespace _5_OpenIdConnect.Controllers
{
    [Route("[controller]/[action]")]
    public class IdentityController : Controller
    {
        [HttpGet]
        [Authorize]
        public async Task<ObjectResult> SignIn()
        {
            return new ObjectResult(User.Claims.Select(c => new { c.Type, c.Value }));

        }

        [AllowAnonymous]
        public async Task SignOut()
        {
            await HttpContext.Authentication.SignOutAsync("Oidc");
            await HttpContext.Authentication.SignOutAsync("Cookies");
        }

        public async Task<IActionResult> CallApi()
        {
            // todo...
            // configure UseIdentityServerBearerTokenAuthentication (or Microsoft middleware)
            // that middleware validates the token

            var token = User.FindFirst("access_token").Value;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetStringAsync("http://localhost:19806/identity");

            ViewBag.Json = JArray.Parse(response).ToString();
            return View();
        }
    }
}
