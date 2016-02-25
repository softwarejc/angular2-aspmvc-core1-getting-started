using Microsoft.AspNet.Mvc;

namespace CookiesAuthentication.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
