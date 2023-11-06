using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [Route("/IfICantAccessThisImKillingMyself")]
        public IActionResult IfICantAccessThisImKillingMyself()
        {
            return Ok("Success");
        }
        [HttpGet]
        [Route("/UserNotLogged")]
        public IActionResult UserNotLogged()
        {
            return StatusCode(401, "user is not logged");
        }
        [HttpGet]
        [Route("/UserNotAuthorized")]
        public IActionResult UserNotAuthorized()
        {
            Response.StatusCode = 403;
            return Content("User not authorized");
        }
    }
}
