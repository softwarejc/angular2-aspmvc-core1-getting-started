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
        public async Task<IActionResult> Public()
        {
            return new ObjectResult("this is public");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Secret()
        {
            return new ObjectResult("this is secret, only fpr authorized");
        }
    }
}
