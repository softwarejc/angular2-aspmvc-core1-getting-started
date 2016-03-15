using Microsoft.AspNet.Mvc;

namespace IdentityClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Account()
        {
            ViewData["Message"] = "Your account page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
