using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace _5_OpenIdConnect.Controllers
{
    [Route("[controller]/[action]")]
    public class InfoController : Controller
    {
        [HttpGet]
        [Authorize]
        public ObjectResult GetClaims()
        {
            return new ObjectResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }

        [AllowAnonymous]
        public async Task SignOut()
        {
            await HttpContext.Authentication.SignOutAsync("Oidc");
            await HttpContext.Authentication.SignOutAsync("Cookies");
        }
    }
}
