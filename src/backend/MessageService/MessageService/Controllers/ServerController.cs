using Microsoft.AspNetCore.Mvc;

namespace MessageService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var value = Environment.GetEnvironmentVariable("APPID");

            return Ok($"Server {value}");
        }
    }
}
