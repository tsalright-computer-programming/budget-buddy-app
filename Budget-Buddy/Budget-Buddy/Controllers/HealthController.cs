using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budget_Buddy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase

    {
        [HttpGet] public IActionResult Get() => Ok(new { status = "ok" });
    }
}
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}