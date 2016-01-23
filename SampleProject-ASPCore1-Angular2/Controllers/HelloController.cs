using System;
using Microsoft.AspNet.Mvc;

namespace GettingStarted_ASPNetCore1_Angular2.Controllers
{
    [Route("api/[controller]")]
    public class HelloController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IActionResult Get(string name)
        {
            var result = $"Hello {name}, the server UTC time is: {DateTime.UtcNow}";
            return new ObjectResult(result);
        }
    }
}
