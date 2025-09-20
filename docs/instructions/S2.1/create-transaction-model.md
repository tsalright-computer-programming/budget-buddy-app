# S2.1 - Create Transaction Model with Migration

## Overview
In this step, you'll create a Transaction model to track individual financial transactions. This builds on what you learned in S1.1 about creating models and migrations, but introduces new concepts like foreign keys and handling money properly.

## What You'll Learn
- How to create models with foreign key relationships
- Why we use cents instead of dollars (avoiding floating-point issues)
- Working with DateOnly for dates
- Creating and applying migrations for new tables

## Why This Matters
- **Foreign Keys**: Connect transactions to categories (like "Groceries" or "Salary")
- **AmountCents**: Store money as integers to avoid rounding errors
- **DateOnly**: Handle dates properly without time components
- **Relationships**: One category can have many transactions

## Prerequisites
- S1.1 and S1.2 completed (Category model and API)
- Your API should be running with Categories endpoints working
- Understanding of migrations and database relationships
- **Git workflow knowledge** - see our [Git Workflow Guide](../general/git-workflow-guide.md)

## Git Workflow Recommendations

### Before Starting S2.1
1. **Create a new branch** for this user story:
   ```bash
   git checkout -b s2.1/transaction-model
   ```

2. **Make sure you're on the correct branch** before making changes

### During Development
- **Commit frequently** as you complete each step (enum, model, DbContext, migration)
- **Use descriptive commit messages** like "Add CategoryType enum"
- **Test thoroughly** before committing

### When S2.1 is Complete
1. **Push your branch** to GitHub
2. **Create a Pull Request** with title "S2.1: Create Transaction Model with Migration"
3. **Link any related issues** in the PR description
4. **Request review** if working with a team

## Step-by-Step Instructions

### 1. Create the Transaction Model

1. **Create a new file** in the `Models` folder:
   - Right-click on the `Models` folder in Solution Explorer
   - Select `Add` → `Class`
   - Name it `Transaction.cs`
   - Click `Add`

2. **Replace the entire contents** with this code:

```csharp
namespace Budget_Buddy.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public DateOnly PostedDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public int AmountCents { get; set; } // positive
        public Guid? CategoryId { get; set; } // null when splits exist later
        public Category? Category { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
```

### 2. Understanding the Transaction Model

Let's break down each property:

- **`Id`**: Unique identifier (GUID) - same as Category
- **`PostedDate`**: When the transaction occurred (DateOnly = just date, no time)
- **`Description`**: What the transaction was for (e.g., "Grocery shopping at Walmart")
- **`AmountCents`**: Amount in cents (e.g., 1500 = $15.00)
- **`CategoryId`**: Which category this belongs to (foreign key)
- **`Category`**: Navigation property to the actual Category object
- **`CreatedUtc`**: When this record was created (audit field)

### 3. Why AmountCents Instead of Amount?

**The Problem with Decimals:**
- Computers can't represent all decimal numbers exactly
- $0.10 + $0.20 might equal $0.3000000001 instead of $0.30
- This causes rounding errors in financial calculations

**The Solution:**
- Store amounts as integers in cents
- $15.00 becomes 1500 cents
- $0.10 becomes 10 cents
- No rounding errors!

### 4. Update AppDbContext

1. **Open `AppDbContext.cs`**
2. **Add the Transactions DbSet** after the Categories line:

```csharp
using Budget_Buddy.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasIndex(c => new { c.Name, c.Type })
            .IsUnique();
    }
}
```

### 5. Create and Apply Migration

Now you'll create a migration to add the Transactions table to your database:

#### Option A: Using Visual Studio (Recommended)

1. **Open Package Manager Console**:
   - Go to `Tools` → `NuGet Package Manager` → `Package Manager Console`

2. **Create the Migration**:
   - Type: `Add-Migration CreateTransactions`
   - Press Enter
   - Wait for it to complete

3. **Apply the Migration**:
   - Type: `Update-Database`
   - Press Enter
   - Wait for it to complete

#### Option B: Using Command Line

1. **Open Command Prompt** and navigate to your project folder
2. **Create the Migration**:
   ```
   dotnet ef migrations add CreateTransactions --project Budget-Buddy\Budget-Buddy.csproj
   ```
3. **Apply the Migration**:
   ```
   dotnet ef database update --project Budget-Buddy\Budget-Buddy.csproj
   ```

### 6. Verify the Migration

1. **Check the Migrations folder**:
   - You should see a new file like `20240101_CreateTransactions.cs`
   - This contains the SQL commands to create the Transactions table

2. **Check your database**:
   - Open your `app.db` file with a SQLite browser
   - **Need help with SQLite?** See our [SQLite Setup and Usage Guide](../general/sqlite-setup-and-usage.md)
   - You should see a new `Transactions` table with these columns:
     - `Id` (TEXT - stores GUID)
     - `PostedDate` (TEXT - stores date as YYYY-MM-DD)
     - `Description` (TEXT)
     - `AmountCents` (INTEGER)
     - `CategoryId` (TEXT - foreign key to Categories)
     - `CreatedUtc` (TEXT - stores datetime)

3. **Test your application**:
   - Run your application (`F5` or `Ctrl+F5`)
   - Make sure it starts without errors
   - Check that Swagger UI loads correctly

## Understanding Database Relationships

### Foreign Key Relationship
- **One Category** can have **Many Transactions**
- This is called a "One-to-Many" relationship
- The `CategoryId` in Transactions points to the `Id` in Categories

### Example Data
```
Categories Table:
Id: 123e4567-e89b-12d3-a456-426614174000
Name: "Groceries"
Type: 2 (Expense)

Transactions Table:
Id: 456e7890-e89b-12d3-a456-426614174001
PostedDate: "2024-01-15"
Description: "Walmart grocery shopping"
AmountCents: 1500 (represents $15.00)
CategoryId: 123e4567-e89b-12d3-a456-426614174000 (points to Groceries)
```

## Common Issues and Solutions

### Issue: "DateOnly not recognized"
**Solution**: Make sure you're using .NET 9.0. DateOnly was introduced in .NET 6, but some features require .NET 9.

### Issue: Migration fails with "table already exists"
**Solution**: 
1. Run `Remove-Migration` in Package Manager Console
2. Delete the `app.db` file
3. Run `Add-Migration CreateTransactions` again
4. Run `Update-Database`

### Issue: "Category not found" in migration
**Solution**: Make sure you have the `using Budget_Buddy.Models;` statement in your `AppDbContext.cs` file.

### Issue: Application won't start after migration
**Solution**:
1. Check the Output window for error messages
2. Make sure all your code changes are saved (`Ctrl+S`)
3. Try building the project first (`Ctrl+Shift+B`)

## What You've Accomplished

By completing this step, you've learned:
- ✅ **How to create models with foreign key relationships**
- ✅ **Why to use cents instead of dollars for money**
- ✅ **How to work with DateOnly for dates**
- ✅ **How to create and apply migrations for new tables**
- ✅ **Understanding database relationships**

## Next Steps

After completing this step:
- Your database will have a Transactions table
- You'll be ready to create the TransactionsController in S2.2
- You'll learn how to create, read, update, and delete transactions
- You'll build filtering and search capabilities

## Key Concepts Learned

### Foreign Keys
- Connect related data across tables
- Ensure data integrity
- Enable powerful queries with joins

### Money Handling
- Always use integers for financial calculations
- Store amounts in the smallest unit (cents)
- Avoid floating-point precision issues

### Date Handling
- Use DateOnly for dates without time
- Store dates in a consistent format
- Handle timezone considerations properly

**Congratulations!** 🎉 You've successfully created the Transaction model and learned important concepts about database relationships and money handling. You're ready to build the API endpoints in S2.2!
