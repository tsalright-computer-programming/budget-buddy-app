using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Budget_Buddy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetController : ControllerBase

    {
        private readonly AppDbContext _context;

        public BudgetController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Query the database to check if it's accessible
                var categoryCount = await _context.Categories.CountAsync();

                return Ok(new
                {
                    status = "ok",
                    database = "connected",
                    budgetItemsCount = categoryCount
,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    status = "error",
                    database = "disconnected",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

    }
}
