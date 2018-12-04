using Microsoft.AspNetCore.Mvc;

namespace User.API.Controllers
{
    [Route("api/[Controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok();
        }
    }
}