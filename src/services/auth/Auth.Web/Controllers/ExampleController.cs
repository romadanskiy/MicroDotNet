using AuthorizationServer.Web.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace AuthorizationServer.Web.Controllers
{
    public class ExampleController : Controller
    {
        [AuthorizeViaBearer]
        public IActionResult Index()
        {
            return Ok("Hello, World!");
        }
    }
}
