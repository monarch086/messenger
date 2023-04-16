using Microsoft.AspNetCore.Mvc;

namespace api_gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var value = Environment.GetEnvironmentVariable("APPID");
        return Ok($"Server {value}");
    }
}