using Microsoft.AspNetCore.Mvc;

namespace Budget_Buddy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController: ControllerBase

    {
        [HttpGet] public IActionResult Get() => Ok(new { status = "ok" });
    }
}
