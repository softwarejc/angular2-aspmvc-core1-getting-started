using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System.Linq;

namespace CookiesAuthentication.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DataController : Controller
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
            return new ObjectResult("this is secret, you can read it only if you are authorized!!");
        }

        [HttpGet]
        [Authorize]
        public IActionResult UserInfo()
        {
            return new ObjectResult(User.Claims.Select(c => new { c.Type, c.Value }));
        }
    }
}
