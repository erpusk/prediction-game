using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RootController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Backend API is running.");
    }
}