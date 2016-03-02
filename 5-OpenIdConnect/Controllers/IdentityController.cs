using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using System.Linq;
using IdentityModel.Client;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace _5_OpenIdConnect.Controllers
{
    [Route("[controller]/[action]")]
    public class IdentityController : Controller
    {
        [HttpGet]
        [Authorize]
        public async Task<ObjectResult> SignIn()
        {
            return new ObjectResult(User.Claims.Select(c => new { c.Type, c.Value }));

        }

        [AllowAnonymous]
        public async Task SignOut()
        {
            await HttpContext.Authentication.SignOutAsync("Oidc");
            await HttpContext.Authentication.SignOutAsync("Cookies");
        }

        public async Task<IActionResult> CallApi()
        {
            // todo...

            var token = User.FindFirst("access_token").Value;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetStringAsync("http://localhost:19806/identity");

            ViewBag.Json = JArray.Parse(response).ToString();
            return View();
        }
    }
}
