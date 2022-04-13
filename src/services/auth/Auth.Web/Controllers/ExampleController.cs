using AuthorizationServer.Web.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Web.Controllers
{
    public class ExampleController : Controller
    {
        [AuthorizeViaBearer]
        public IActionResult Index()
        {
            return Ok("Hello, World!");
        }

        [HttpGet("/ocelot")]
        public IActionResult GetOcelot()
        {
            return Ok("Hello, Ocelot!");
        }
    }
}
