using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IdentityResourceAPI.Controllers
{
    public class TicketsController : ApiController
    {
        private static readonly List<string> Tickets = new List<string> { "Do ABC!", "XYZ not working..." };

        [HttpGet]
        public HttpResponseMessage ApiInfo()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new[] { "api/tickets/read", "api/tickets/add?{ticket}" });
        }

        [HttpGet]
        public HttpResponseMessage Read()
        {
            return Request.CreateResponse(HttpStatusCode.OK, Tickets);
        }

        [HttpGet]
        public HttpResponseMessage Add(string ticket)
        {
            Tickets.Add(ticket);

            return Request.CreateResponse(HttpStatusCode.Created, Tickets);
        }
    }
}
