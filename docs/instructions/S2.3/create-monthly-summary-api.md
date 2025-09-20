# S2.3 - Create Monthly Summary API

## Overview
In this step, you'll create a Monthly Summary API that aggregates transaction data to show income, expenses, and net totals for a given month. This builds on what you learned in S2.2 about filtering and joins, but adds data aggregation and reporting capabilities.

## What You'll Learn
- How to aggregate data using LINQ
- Grouping and summing data by category types
- Creating summary reports
- Working with date ranges for monthly data
- Building efficient aggregation queries

## Why This Matters
- **Financial Reporting**: Users need to see their monthly financial summary
- **Data Aggregation**: Combine many transactions into meaningful totals
- **Business Logic**: Calculate net income (income - expenses)
- **User Experience**: Provide quick insights into financial health

## Prerequisites
- S2.2 completed (Transactions CRUD API)
- Your API should be running with all endpoints working
- Understanding of data aggregation and LINQ queries
- **Git workflow knowledge** - see our [Git Workflow Guide](../general/git-workflow-guide.md)

## Git Workflow Recommendations

### Before Starting S2.3
1. **Create a new branch** for this user story:
   ```bash
   git checkout -b s2.3/monthly-summary-api
   ```

2. **Make sure you're on the correct branch** before making changes

### During Development
- **Commit frequently** as you complete each step (DTOs, controller, aggregation logic, testing)
- **Use descriptive commit messages** like "Add MonthlySummaryDto"
- **Test thoroughly** before committing

### When S2.3 is Complete
1. **Push your branch** to GitHub
2. **Create a Pull Request** with title "S2.3: Create Monthly Summary API"
3. **Link any related issues** in the PR description
4. **Request review** if working with a team
5. **This completes the backend API!** Ready for frontend development

## Step-by-Step Instructions

### 1. Create Summary DTOs

1. **Create a new file** in the `DTOs` folder:
   - Right-click on the `DTOs` folder in Solution Explorer
   - Select `Add` → `Class`
   - Name it `SummaryDtos.cs`
   - Click `Add`

2. **Replace the entire contents** with this code:

```csharp
namespace Budget_Buddy.DTOs
{
    public record MonthlySummaryDto(
        string Month,
        int IncomeCents,
        int ExpenseCents,
        int NetCents
    );
}
```

### 2. Understanding the Summary DTO

**MonthlySummaryDto** - Contains the monthly financial summary:
- `Month`: The month being summarized (e.g., "2024-01")
- `IncomeCents`: Total income for the month in cents
- `ExpenseCents`: Total expenses for the month in cents
- `NetCents`: Net amount (income - expenses) in cents

### 3. Create SummaryController

1. **Create a new controller**:
   - Right-click on the `Controllers` folder in Solution Explorer
   - Select `Add` → `Controller`
   - Choose `API Controller - Empty`
   - Name it `SummaryController`
   - Click `Add`

2. **Replace the entire contents** with this code:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Budget_Buddy.Data;
using Budget_Buddy.DTOs;

namespace Budget_Buddy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SummaryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SummaryController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/summary?month=2024-01
        [HttpGet]
        public async Task<IActionResult> GetMonthlySummary([FromQuery] string month)
        {
            // Validate month format
            if (string.IsNullOrEmpty(month))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Month parameter is required (format: YYYY-MM)",
                    Status = 400
                });
            }

            // Parse month to get start and end dates
            if (!DateTime.TryParseExact(month + "-01", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var monthStart))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Month must be in YYYY-MM format (e.g., 2024-01)",
                    Status = 400
                });
            }

            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            // Convert to DateOnly for database query
            var startDate = DateOnly.FromDateTime(monthStart);
            var endDate = DateOnly.FromDateTime(monthEnd);

            // Get all transactions for the month with their categories
            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.PostedDate >= startDate && t.PostedDate <= endDate)
                .ToListAsync();

            // Group by category type and sum amounts
            var incomeCents = transactions
                .Where(t => t.Category != null && t.Category.Type == CategoryType.Income)
                .Sum(t => t.AmountCents);

            var expenseCents = transactions
                .Where(t => t.Category != null && t.Category.Type == CategoryType.Expense)
                .Sum(t => t.AmountCents);

            // Calculate net (income - expenses)
            var netCents = incomeCents - expenseCents;

            // Create and return the summary
            var summary = new MonthlySummaryDto(
                month,
                incomeCents,
                expenseCents,
                netCents
            );

            return Ok(summary);
        }
    }
}
```

### 4. Understanding the Aggregation Logic

Let's break down how the summary is calculated:

1. **Parse the month parameter**: Convert "2024-01" to actual dates
2. **Filter transactions**: Get all transactions within the month
3. **Group by category type**: Separate income vs expense transactions
4. **Sum amounts**: Calculate totals for each type
5. **Calculate net**: Income - Expenses

### 5. Test Your API

#### Method 1: Using Swagger UI (Recommended)

1. **Run your application** (`F5` or `Ctrl+F5`)
2. **Navigate to Swagger UI** (add `/swagger` to your URL)
3. **Look for the "Summary" section**

#### Test the GET Endpoint

1. **Click on `GET /api/summary`**
2. **Click "Try it out"**
3. **Enter a month parameter**: `2024-01`
4. **Click "Execute"**
5. **You should see a response like**:
```json
{
  "month": "2024-01",
  "incomeCents": 500000,
  "expenseCents": 350000,
  "netCents": 150000
}
```

#### Test with Different Months

Try different month values:
- `2024-01` - January 2024
- `2024-02` - February 2024
- `2023-12` - December 2023

### 6. Understanding the Response

The response shows:
- **month**: The month you requested
- **incomeCents**: Total income in cents (500000 = $5,000.00)
- **expenseCents**: Total expenses in cents (350000 = $3,500.00)
- **netCents**: Net amount in cents (150000 = $1,500.00)

### 7. Test with Real Data

To see meaningful results, you'll need some transactions:

1. **Create some categories** using the CategoryController
2. **Create some transactions** using the TransactionsController
3. **Make sure some are income and some are expenses**
4. **Test the summary API** to see the totals

## Advanced Features to Try

### 1. Add Error Handling for Empty Months

If there are no transactions for a month, the API will return zeros. This is correct behavior, but you might want to add a note:

```csharp
// Add this after calculating the summary
var totalTransactions = transactions.Count;
if (totalTransactions == 0)
{
    // You could add a note or handle this differently
    // For now, we'll just return zeros
}
```

### 2. Add More Detailed Summary

You could extend the DTO to include more details:

```csharp
public record DetailedMonthlySummaryDto(
    string Month,
    int IncomeCents,
    int ExpenseCents,
    int NetCents,
    int TransactionCount,
    int IncomeTransactionCount,
    int ExpenseTransactionCount
);
```

### 3. Add Yearly Summary

You could add an endpoint for yearly summaries:

```csharp
[HttpGet("yearly")]
public async Task<IActionResult> GetYearlySummary([FromQuery] int year)
{
    // Similar logic but for the entire year
}
```

## Common Issues and Solutions

### Issue: "Month parameter is required" error
**Solution**: Make sure you're passing the month parameter in the query string: `?month=2024-01`

### Issue: "Month must be in YYYY-MM format" error
**Solution**: Use exactly the format "YYYY-MM" (e.g., "2024-01", not "2024-1" or "01-2024")

### Issue: All values are zero
**Solution**: 
1. Make sure you have transactions in that month
2. Check that transactions have valid categories
3. Verify the date range is correct

### Issue: "CategoryType not recognized"
**Solution**: Make sure you have `using Budget_Buddy.Models;` at the top of your controller.

### Issue: Date parsing fails
**Solution**: The month parameter must be in exactly "YYYY-MM" format (e.g., "2024-01").

## What You've Accomplished

By completing this step, you've learned:
- ✅ **How to aggregate data using LINQ**
- ✅ **Grouping and summing data by category types**
- ✅ **Creating summary reports**
- ✅ **Working with date ranges for monthly data**
- ✅ **Building efficient aggregation queries**
- ✅ **Financial calculation logic**

## Next Steps

After completing this step:
- You'll have a complete financial tracking system
- You'll understand how to build reporting APIs
- You'll be ready to add more advanced features
- You'll have the foundation for a full budget application

## Key Concepts Learned

### Data Aggregation
- Combine multiple records into summary data
- Use LINQ to group and sum data
- Handle empty result sets gracefully

### Financial Calculations
- Calculate income vs expenses
- Determine net financial position
- Handle money in cents to avoid rounding errors

### Date Range Queries
- Filter data by month boundaries
- Parse and validate date parameters
- Handle different date formats

### API Design
- Design clear, intuitive endpoints
- Provide meaningful error messages
- Return data in a useful format

**Congratulations!** 🎉 You've successfully created a Monthly Summary API and learned how to aggregate financial data. You now have a complete budget tracking system with categories, transactions, and reporting capabilities!

## What You've Built

Your Budget Buddy application now includes:
- ✅ **Category Management** (S1.1, S1.2)
- ✅ **Transaction Tracking** (S2.1, S2.2)
- ✅ **Monthly Financial Reports** (S2.3)
- ✅ **Full CRUD APIs** for all entities
- ✅ **Data validation and error handling**
- ✅ **Database relationships and joins**
- ✅ **Filtering and search capabilities**

You've built a solid foundation for a personal finance application!
