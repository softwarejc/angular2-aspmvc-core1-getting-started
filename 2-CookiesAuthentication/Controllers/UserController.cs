using Microsoft.AspNet.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using Security_ASPNetCore1_JwtBearer;

namespace Security_ASPNetCore1_Angular2.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {

        [HttpGet]
        public IActionResult Public()
        {
            return new ObjectResult("this is public");
        }

        [HttpGet]
        [Authorize(ActiveAuthenticationSchemes = SecurityConfig.Scheme )]
        public IActionResult Secret()
        {
            return new ObjectResult(GetUserIdentityAndClaims());
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "Juan Carlos"),
            }, SecurityConfig.Scheme);

            var user = HttpContext.User as ClaimsPrincipal;
            //user.AddIdentity(identity);

            await HttpContext.Authentication.SignInAsync(SecurityConfig.Scheme, new ClaimsPrincipal(identity));
            return new ObjectResult(GetUserIdentityAndClaims());
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync(SecurityConfig.Scheme);
            return new ObjectResult("logged OUT");
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
