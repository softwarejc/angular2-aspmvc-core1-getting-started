using System;
using Microsoft.AspNet.Mvc;

namespace GettingStarted_ASPNetCore1_Angular2.Controllers
{
    [Route("api/[controller]")]
    public class HelloController : Controller
    {
        [HttpGet]
        public IActionResult Get(string name)
        {
            var time = DateTime.UtcNow.ToString("hh:mm:ss");

            var response = new
            {
                message = $"{time} - Hello {name}, server-side speaking!"
            };

            return new ObjectResult(response);
        }
    }
}
