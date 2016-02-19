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
            if (string.IsNullOrEmpty(name))
            {
                return HttpBadRequest("name is empty");
            }

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
            return new ObjectResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }
}
