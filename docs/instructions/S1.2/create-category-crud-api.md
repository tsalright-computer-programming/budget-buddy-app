# S1.2 - Create Category CRUD API

## Overview
In this step, you'll create a proper CategoryController to replace the BudgetController and implement full CRUD operations for categories. This includes creating DTOs, implementing validation, and handling soft deletes.

## Refactoring Strategy

### Remove BudgetController
We're replacing the BudgetController with a proper CategoryController that follows RESTful conventions and handles category-specific operations.

### Create CategoryController
The new controller will handle:
- Creating categories
- Listing categories (with filtering)
- Updating categories
- Soft deleting categories (archiving)

## Prerequisites
- S1.1 completed (Category model refactoring)
- Your API should be running with the updated Category model
- Understanding of DTOs, controllers, and CRUD operations
- **Git workflow knowledge** - see our [Git Workflow Guide](../general/git-workflow-guide.md)

## Git Workflow Recommendations

### Before Starting S1.2
1. **Create a new branch** for this user story:
   ```bash
   git checkout -b s1.2/category-crud-api
   ```

2. **Make sure you're on the correct branch** before making changes

### During Development
- **Commit frequently** as you complete each step (DTOs, controller, endpoints, testing)
- **Use descriptive commit messages** like "Add Category DTOs"
- **Test thoroughly** before committing

### When S1.2 is Complete
1. **Push your branch** to GitHub
2. **Create a Pull Request** with title "S1.2: Create Category CRUD API"
3. **Link any related issues** in the PR description
4. **Request review** if working with a team

## Step-by-Step Instructions

### 1. Create DTOs (Data Transfer Objects)

First, we need to create a DTOs folder and file in your project:

1. **Create the DTOs folder**:
   - In Visual Studio, right-click on your project in Solution Explorer
   - Select `Add` → `New Folder`
   - Name the folder `DTOs`

2. **Create the CategoryDtos.cs file**:
   - Right-click on the `DTOs` folder you just created
   - Select `Add` → `Class`
   - Name it `CategoryDtos.cs`
   - Click `Add`

3. **Replace the contents** of the new file with this code:

```csharp
using Budget_Buddy.Models;

namespace Budget_Buddy.DTOs
{
    public record CategoryCreateDto(string Name, CategoryType Type);
    public record CategoryReadDto(Guid Id, string Name, CategoryType Type, bool IsArchived);
    public record CategoryUpdateDto(string Name, CategoryType Type, bool IsArchived);
}
```

### 2. Delete the BudgetController

1. **Find the BudgetController**:
   - In Visual Studio Solution Explorer, navigate to `Controllers` folder
   - Find `BudgetController.cs`

2. **Delete the file**:
   - Right-click on `BudgetController.cs`
   - Select `Delete`
   - Click `OK` to confirm

### 3. Create CategoryController

1. **Create the new controller file**:
   - Right-click on the `Controllers` folder in Solution Explorer
   - Select `Add` → `Controller`
   - Choose `API Controller - Empty`
   - Name it `CategoryController`
   - Click `Add`

2. **Replace the entire contents** of the new file with this code:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Budget_Buddy.Data;
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
```

### 4. Test Your API

Now let's test your new CategoryController. There are several ways to test it:

#### Method 1: Using Swagger UI (Easiest for beginners)

1. **Run your application**:
   - Press `F5` or `Ctrl+F5` in Visual Studio
   - Your browser should open with the Swagger UI

2. **Test the endpoints**:
   - You should see a new "Category" section in the Swagger UI
   - Click on `POST /api/category` to expand it
   - Click "Try it out"
   - In the request body, enter:
   ```json
   {
     "name": "Groceries",
     "type": 2
   }
   ```
   - Click "Execute"
   - You should get a 201 Created response with the new category

3. **Test other endpoints**:
   - Try `GET /api/category` to see all categories
   - Try `GET /api/category?type=2` to filter by expense type
   - Try `PUT /api/category/{id}` to update a category
   - Try `DELETE /api/category/{id}` to soft delete a category

#### Method 2: Using Postman (More advanced)

1. **Download Postman** from postman.com if you don't have it
2. **Create a new request** for each endpoint:

**Create a Category**:
- Method: POST
- URL: `https://localhost:7xxx/api/category` (replace 7xxx with your port)
- Headers: `Content-Type: application/json`
- Body (raw JSON):
```json
{
  "name": "Groceries",
  "type": 2
}
```

**Get All Categories**:
- Method: GET
- URL: `https://localhost:7xxx/api/category`

**Get Categories by Type**:
- Method: GET
- URL: `https://localhost:7xxx/api/category?type=2`

**Update a Category**:
- Method: PUT
- URL: `https://localhost:7xxx/api/category/{id}` (replace {id} with actual ID)
- Headers: `Content-Type: application/json`
- Body (raw JSON):
```json
{
  "name": "Updated Groceries",
  "type": 2,
  "isArchived": false
}
```

**Delete a Category**:
- Method: DELETE
- URL: `https://localhost:7xxx/api/category/{id}` (replace {id} with actual ID)

#### Method 3: Using curl (Command line)

If you prefer command line, you can use curl commands:

```bash
# Create a category
curl -X POST "https://localhost:7xxx/api/category" \
  -H "Content-Type: application/json" \
  -d '{"name": "Groceries", "type": 2}'

# Get all categories
curl -X GET "https://localhost:7xxx/api/category"

# Get categories by type
curl -X GET "https://localhost:7xxx/api/category?type=2"

# Update a category (replace {id} with actual ID)
curl -X PUT "https://localhost:7xxx/api/category/{id}" \
  -H "Content-Type: application/json" \
  -d '{"name": "Updated Groceries", "type": 2, "isArchived": false}'

# Delete a category (replace {id} with actual ID)
curl -X DELETE "https://localhost:7xxx/api/category/{id}"
```

## Understanding the Implementation

### DTOs (Data Transfer Objects)
- **CategoryCreateDto**: For creating new categories
- **CategoryReadDto**: For returning category data
- **CategoryUpdateDto**: For updating existing categories

### Validation Rules
- Name is required and must be 2-50 characters
- Name is trimmed of whitespace
- Unique constraint on (Name, Type) combination
- Proper error responses using ProblemDetails

### HTTP Status Codes
- **201 Created**: When a category is successfully created
- **200 OK**: When data is successfully retrieved or updated
- **204 No Content**: When a category is successfully deleted
- **400 Bad Request**: When validation fails
- **404 Not Found**: When a category doesn't exist

### Soft Delete
Instead of actually deleting records, we set `IsArchived = true`. This preserves data integrity and allows for recovery.

## Verification Steps

1. **Test all endpoints** using Postman, curl, or your API testing tool
2. **Verify validation** by sending invalid data
3. **Check uniqueness** by trying to create duplicate categories
4. **Test soft delete** by deleting a category and then checking if it's archived
5. **Check the database** to see your data:
   - Open your `app.db` file with a SQLite browser
   - **Need help with SQLite?** See our [SQLite Setup and Usage Guide](../general/sqlite-setup-and-usage.md)
   - Look at the Categories table to see your created categories

## Common Issues and Solutions

### Issue: DTOs not recognized
**Solution**: Make sure you've created the DTOs file and added the proper using statement.

### Issue: Validation not working
**Solution**: Check that you're sending the correct JSON format and that the validation logic is properly implemented.

### Issue: Unique constraint violations
**Solution**: The database will enforce the unique constraint, but we also check in the application for better error messages.

## What We've Learned

This implementation teaches us:
- How to create proper RESTful APIs
- The importance of validation and error handling
- How to use DTOs for data transfer
- Soft delete patterns for data preservation
- Proper HTTP status codes and responses
