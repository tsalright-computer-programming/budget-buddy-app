using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Budget_Buddy.Models;
using Budget_Buddy.DTOs;

namespace Budget_Buddy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        // POST /api/transactions
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreateDto dto)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(dto.Description))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Description is required",
                    Status = 400
                });
            }

            if (dto.Description.Length < 1 || dto.Description.Length > 100)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Description must be between 1 and 100 characters",
                    Status = 400
                });
            }

            if (dto.AmountCents < 1)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Amount must be at least 1 cent",
                    Status = 400
                });
            }

            // Parse and validate date
            if (!DateOnly.TryParseExact(dto.PostedDate, "yyyy-MM-dd", out var postedDate))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "PostedDate must be in YYYY-MM-DD format",
                    Status = 400
                });
            }

            if (postedDate > DateOnly.FromDateTime(DateTime.Today))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "PostedDate cannot be in the future",
                    Status = 400
                });
            }

            // Check if category exists
            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Category not found",
                    Status = 400
                });
            }

            // Create transaction
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                PostedDate = postedDate,
                Description = dto.Description.Trim(),
                AmountCents = dto.AmountCents,
                CategoryId = dto.CategoryId,
                CreatedUtc = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            // Return the created transaction with category info
            var readDto = new TransactionReadDto(
                transaction.Id,
                transaction.PostedDate.ToString("yyyy-MM-dd"),
                transaction.Description,
                transaction.AmountCents,
                transaction.CategoryId,
                category.Name,
                category.Type
            );

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, readDto);
        }

        // GET /api/transactions?from=2024-01-01&to=2024-01-31&categoryId=xxx&type=1
        [HttpGet]
        public async Task<IActionResult> GetTransactions(
            [FromQuery] string? from = null,
            [FromQuery] string? to = null,
            [FromQuery] Guid? categoryId = null,
            [FromQuery] CategoryType? type = null)
        {
            var query = _context.Transactions
                .Include(t => t.Category)
                .AsQueryable();

            // Filter by date range
            if (!string.IsNullOrEmpty(from) && DateOnly.TryParseExact(from, "yyyy-MM-dd", out var fromDate))
            {
                query = query.Where(t => t.PostedDate >= fromDate);
            }

            if (!string.IsNullOrEmpty(to) && DateOnly.TryParseExact(to, "yyyy-MM-dd", out var toDate))
            {
                query = query.Where(t => t.PostedDate <= toDate);
            }

            // Filter by category
            if (categoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == categoryId.Value);
            }

            // Filter by category type
            if (type.HasValue)
            {
                query = query.Where(t => t.Category != null && t.Category.Type == type.Value);
            }

            var transactions = await query
                .OrderByDescending(t => t.PostedDate)
                .ThenBy(t => t.Description)
                .Select(t => new TransactionReadDto(
                    t.Id,
                    t.PostedDate.ToString("yyyy-MM-dd"),
                    t.Description,
                    t.AmountCents,
                    t.CategoryId,
                    t.Category != null ? t.Category.Name : null,
                    t.Category != null ? t.Category.Type : null
                ))
                .ToListAsync();

            return Ok(transactions);
        }

        // GET /api/transactions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(Guid id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Transaction with ID {id} was not found",
                    Status = 404
                });
            }

            var readDto = new TransactionReadDto(
                transaction.Id,
                transaction.PostedDate.ToString("yyyy-MM-dd"),
                transaction.Description,
                transaction.AmountCents,
                transaction.CategoryId,
                transaction.Category?.Name,
                transaction.Category?.Type
            );

            return Ok(readDto);
        }

        // PUT /api/transactions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(Guid id, [FromBody] TransactionUpdateDto dto)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Transaction with ID {id} was not found",
                    Status = 404
                });
            }

            // Validation (same as create)
            if (string.IsNullOrWhiteSpace(dto.Description))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Description is required",
                    Status = 400
                });
            }

            if (dto.Description.Length < 1 || dto.Description.Length > 100)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Description must be between 1 and 100 characters",
                    Status = 400
                });
            }

            if (dto.AmountCents < 1)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Amount must be at least 1 cent",
                    Status = 400
                });
            }

            if (!DateOnly.TryParseExact(dto.PostedDate, "yyyy-MM-dd", out var postedDate))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "PostedDate must be in YYYY-MM-DD format",
                    Status = 400
                });
            }

            if (postedDate > DateOnly.FromDateTime(DateTime.Today))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "PostedDate cannot be in the future",
                    Status = 400
                });
            }

            // Check if category exists
            var category = await _context.Categories.FindAsync(dto.CategoryId);
            if (category == null)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Category not found",
                    Status = 400
                });
            }

            // Update transaction
            transaction.PostedDate = postedDate;
            transaction.Description = dto.Description.Trim();
            transaction.AmountCents = dto.AmountCents;
            transaction.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();

            var readDto = new TransactionReadDto(
                transaction.Id,
                transaction.PostedDate.ToString("yyyy-MM-dd"),
                transaction.Description,
                transaction.AmountCents,
                transaction.CategoryId,
                category.Name,
                category.Type
            );

            return Ok(readDto);
        }

        // DELETE /api/transactions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Transaction with ID {id} was not found",
                    Status = 404
                });
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
