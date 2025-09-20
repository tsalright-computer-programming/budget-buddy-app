# S2.2 - Create Transactions CRUD API

## Overview
In this step, you'll create a TransactionsController with full CRUD operations and filtering capabilities. This builds on what you learned in S1.2 with the CategoryController, but adds more complex features like date parsing, filtering, and joins.

## What You'll Learn
- How to create DTOs for complex data with relationships
- Date parsing and validation
- Advanced filtering with multiple parameters
- Database joins to include related data
- More sophisticated validation rules

## Why This Matters
- **Filtering**: Users need to find transactions by date, category, or type
- **Relationships**: Show category names and types in transaction lists
- **Date Handling**: Parse and validate dates properly
- **Complex Validation**: Multiple validation rules working together

## Prerequisites
- S2.1 completed (Transaction model and migration)
- Your API should be running with all endpoints working
- Understanding of DTOs, controllers, and API design
- **Git workflow knowledge** - see our [Git Workflow Guide](../general/git-workflow-guide.md)

## Git Workflow Recommendations

### Before Starting S2.2
1. **Create a new branch** for this user story:
   ```bash
   git checkout -b s2.2/transactions-crud-api
   ```

2. **Make sure you're on the correct branch** before making changes

### During Development
- **Commit frequently** as you complete each step (DTOs, controller, endpoints, testing)
- **Use descriptive commit messages** like "Add Transaction DTOs"
- **Test thoroughly** before committing

### When S2.2 is Complete
1. **Push your branch** to GitHub
2. **Create a Pull Request** with title "S2.2: Create Transactions CRUD API"
3. **Link any related issues** in the PR description
4. **Request review** if working with a team

## Step-by-Step Instructions

### 1. Create Transaction DTOs

1. **Create a new file** in the `DTOs` folder:
   - Right-click on the `DTOs` folder in Solution Explorer
   - Select `Add` → `Class`
   - Name it `TransactionDtos.cs`
   - Click `Add`

2. **Replace the entire contents** with this code:

```csharp
using Budget_Buddy.Models;

namespace Budget_Buddy.DTOs
{
    public record TransactionCreateDto(string PostedDate, string Description, int AmountCents, Guid CategoryId);
    
    public record TransactionReadDto(
        Guid Id, 
        string PostedDate, 
        string Description, 
        int AmountCents,
        Guid? CategoryId, 
        string? CategoryName, 
        CategoryType? CategoryType
    );
    
    public record TransactionUpdateDto(string PostedDate, string Description, int AmountCents, Guid CategoryId);
}
```

### 2. Understanding the DTOs

**TransactionCreateDto** - For creating new transactions:
- `PostedDate`: Date as string (e.g., "2024-01-15")
- `Description`: What the transaction was for
- `AmountCents`: Amount in cents (positive integer)
- `CategoryId`: Which category this belongs to

**TransactionReadDto** - For returning transaction data:
- Includes all create fields plus:
- `Id`: Unique identifier
- `CategoryName`: Name of the category (from join)
- `CategoryType`: Type of the category (Income/Expense)

**TransactionUpdateDto** - For updating transactions:
- Same as create, but we'll add the Id from the URL

### 3. Create TransactionsController

1. **Create a new controller**:
   - Right-click on the `Controllers` folder in Solution Explorer
   - Select `Add` → `Controller`
   - Choose `API Controller - Empty`
   - Name it `TransactionsController`
   - Click `Add`

2. **Replace the entire contents** with this code:

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
```

### 4. Test Your API

Now let's test all the endpoints using Swagger UI:

#### Method 1: Using Swagger UI (Recommended)

1. **Run your application** (`F5` or `Ctrl+F5`)
2. **Navigate to Swagger UI** (add `/swagger` to your URL)
3. **Look for the "Transactions" section**

#### Test the POST Endpoint

1. **Click on `POST /api/transactions`**
2. **Click "Try it out"**
3. **Enter this test data**:
```json
{
  "postedDate": "2024-01-15",
  "description": "Grocery shopping at Walmart",
  "amountCents": 1500,
  "categoryId": "your-category-id-here"
}
```
4. **Click "Execute"**
5. **You should get a 201 Created response**

#### Test the GET Endpoint

1. **Click on `GET /api/transactions`**
2. **Click "Try it out"**
3. **Try different filter combinations**:
   - `from`: "2024-01-01"
   - `to`: "2024-01-31"
   - `categoryId`: (leave empty or enter a category ID)
   - `type`: 1 (for Income) or 2 (for Expense)
4. **Click "Execute"**
5. **You should see a list of transactions**

### 5. Understanding the Filtering

The GET endpoint supports multiple filters:

- **Date Range**: `from` and `to` parameters
- **Category**: `categoryId` parameter
- **Type**: `type` parameter (1 = Income, 2 = Expense)

**Example URLs:**
- `GET /api/transactions` - All transactions
- `GET /api/transactions?from=2024-01-01&to=2024-01-31` - January 2024
- `GET /api/transactions?type=2` - Only expenses
- `GET /api/transactions?categoryId=123&from=2024-01-01` - Specific category in January

### 6. Understanding the Validation

The API validates:

- **Description**: Required, 1-100 characters
- **AmountCents**: Must be at least 1
- **PostedDate**: Must be in YYYY-MM-DD format, not in the future
- **CategoryId**: Must exist in the database

## Common Issues and Solutions

### Issue: "DateOnly not recognized"
**Solution**: Make sure you're using .NET 9.0 and have the correct using statements.

### Issue: "Include not recognized"
**Solution**: Make sure you have `using Microsoft.EntityFrameworkCore;` at the top of your controller.

### Issue: "Category not found" when creating transactions
**Solution**: Make sure you have some categories created first. Use the CategoryController to create categories before creating transactions.

### Issue: Date parsing fails
**Solution**: Make sure dates are in exactly "YYYY-MM-DD" format (e.g., "2024-01-15").

### Issue: Filtering not working
**Solution**: Check that your query parameters match exactly (case-sensitive).

## What You've Accomplished

By completing this step, you've learned:
- ✅ **How to create complex DTOs with relationships**
- ✅ **Date parsing and validation**
- ✅ **Advanced filtering with multiple parameters**
- ✅ **Database joins to include related data**
- ✅ **Sophisticated validation rules**
- ✅ **Building comprehensive CRUD APIs**

## Next Steps

After completing this step:
- You'll have a fully functional Transactions API
- You'll be ready to build the Monthly Summary API in S2.3
- You'll learn how to aggregate data and create reports
- You'll understand how to build complex queries

## Key Concepts Learned

### DTOs with Relationships
- Include related data in responses
- Separate concerns between models and DTOs
- Handle nullable relationships properly

### Advanced Filtering
- Multiple filter parameters
- Date range filtering
- Category and type filtering
- Efficient database queries

### Date Handling
- Parse dates from strings
- Validate date formats
- Handle date ranges properly
- Prevent future dates

**Congratulations!** 🎉 You've successfully created a comprehensive Transactions API with filtering capabilities. You're ready to build the Monthly Summary API in S2.3!
