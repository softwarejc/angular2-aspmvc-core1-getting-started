using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

namespace _5_OpenIdConnect.Controllers
{
    [Route("[controller]")]
    public class IdentityController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            return new JsonResult("it works!");
        }
    }
}
