using Microsoft.AspNet.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;

namespace Security_ASPNetCore1_Angular2.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Login(string name)
        {
            await CookieMonsterSecurity.SignIn(HttpContext.Authentication, name);

            return new ObjectResult($"{name} logged IN");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await CookieMonsterSecurity.SignOut(HttpContext.Authentication);

            return new ObjectResult("logged OUT");
        }

        [HttpGet]
        [Authorize(CookieMonsterSecurity.OnlyGoodMonstersPolicy)]
        public IActionResult Info()
        {
            return GetUserIdentityAndClaims();
        }

        private IActionResult GetUserIdentityAndClaims()
        {
            var authenticatedType = HttpContext.User.Identities
                .Where(identity => identity.IsAuthenticated)
                .Select(identity => identity.AuthenticationType);

            var claims = User.Claims.Select(c => new { c.Type, c.Value });

            return new ObjectResult(new { authenticatedType, claims });
        }
    }
}
