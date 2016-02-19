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
            return new ObjectResult("this is public...");
        }

        [HttpGet]
        [Authorize(SecurityHelper.AuthenticatedUserPolicy)]
        public IActionResult Secret()
        {
            return new ObjectResult("this is secret!!");
        }

        [HttpGet]
        public IActionResult Info()
        {
            return new ObjectResult(GetUserIdentityAndClaims());
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await SecurityHelper.SignIn(HttpContext.Authentication, "Juan Carlos");
            return new ObjectResult("logged IN");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await SecurityHelper.SignOut(HttpContext.Authentication);
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
