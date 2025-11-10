using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Budget_Buddy.Models;
using Budget_Buddy.DTOs;

namespace Budget_Buddy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // POST /api/category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto dto)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Name is required",
                    Status = 400
                });
            }

            var trimmedName = dto.Name.Trim();
            if (trimmedName.Length < 2 || trimmedName.Length > 50)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Name must be between 2 and 50 characters",
                    Status = 400
                });
            }

            // Check for uniqueness
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == trimmedName && c.Type == dto.Type);

            if (existingCategory != null)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = $"A category with name '{trimmedName}' and type '{dto.Type}' already exists",
                    Status = 400
                });
            }

            // Create new category
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = trimmedName,
                Type = dto.Type,
                IsArchived = false,
                CreatedUtc = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var readDto = new CategoryReadDto(category.Id, category.Name, category.Type, category.IsArchived);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, readDto);
        }

        // GET /api/category?type=Income&includeArchived=false
        [HttpGet]
        public async Task<IActionResult> GetCategories(
            [FromQuery] CategoryType? type = null,
            [FromQuery] bool includeArchived = false)
        {
            var query = _context.Categories.AsQueryable();

            if (type.HasValue)
            {
                query = query.Where(c => c.Type == type.Value);
            }

            if (!includeArchived)
            {
                query = query.Where(c => !c.IsArchived);
            }

            var categories = await query
                .OrderBy(c => c.Type)
                .ThenBy(c => c.Name)
                .Select(c => new CategoryReadDto(c.Id, c.Name, c.Type, c.IsArchived))
                .ToListAsync();

            return Ok(categories);
        }

        // GET /api/category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Category with ID {id} was not found",
                    Status = 404
                });
            }

            var readDto = new CategoryReadDto(category.Id, category.Name, category.Type, category.IsArchived);
            return Ok(readDto);
        }

        // PUT /api/category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryUpdateDto dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Category with ID {id} was not found",
                    Status = 404
                });
            }

            // Validation
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Name is required",
                    Status = 400
                });
            }

            var trimmedName = dto.Name.Trim();
            if (trimmedName.Length < 2 || trimmedName.Length > 50)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Name must be between 2 and 50 characters",
                    Status = 400
                });
            }

            // Check for uniqueness (excluding current category)
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == trimmedName && c.Type == dto.Type && c.Id != id);

            if (existingCategory != null)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = $"A category with name '{trimmedName}' and type '{dto.Type}' already exists",
                    Status = 400
                });
            }

            // Update category
            category.Name = trimmedName;
            category.Type = dto.Type;
            category.IsArchived = dto.IsArchived;

            await _context.SaveChangesAsync();

            var readDto = new CategoryReadDto(category.Id, category.Name, category.Type, category.IsArchived);
            return Ok(readDto);
        }

        // DELETE /api/category/{id} (soft delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Category with ID {id} was not found",
                    Status = 404
                });
            }

            // Soft delete (archive)
            category.IsArchived = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
