# S1.1 - Refactor to Category Model with Migration

## Overview
In this step, you'll refactor your existing Category model to match the proper database design and create a migration to update your database schema. This is a great lesson in refactoring - we're improving our initial design based on better requirements.

## Current State vs. Target State

### What We Have Now
- Simple Category model with basic properties
- BudgetController that was created for database testing
- Basic database connection

### What We're Building
- Proper Category model with GUID, validation, and business rules
- CategoryType enum for type safety
- Unique constraint on (Name, Type) combination
- Migration to update the database schema

## Prerequisites
- S0.1, S0.2, and S0.3 completed
- Your API should be running with basic database connection
- Understanding of models, migrations, and database concepts
- **Git workflow knowledge** - see our [Git Workflow Guide](../general/git-workflow-guide.md)

## Git Workflow Recommendations

### Before Starting S1.1
1. **Create a new branch** for this user story:
   ```bash
   git checkout -b s1.1/refactor-category-model
   ```

2. **Make sure you're on the correct branch** before making changes

### During Development
- **Commit frequently** as you complete each step (enum, model, DbContext, migration)
- **Use descriptive commit messages** like "Add CategoryType enum"
- **Test thoroughly** before committing

### When S1.1 is Complete
1. **Push your branch** to GitHub
2. **Create a Pull Request** with title "S1.1: Refactor to Category Model with Migration"
3. **Link any related issues** in the PR description
4. **Request review** if working with a team

## Step-by-Step Instructions

### 1. Create the CategoryType Enum

Create a new file `CategoryType.cs` in the `Models` folder:

```csharp
namespace Budget_Buddy.Models
{
    public enum CategoryType 
    { 
        Income = 1, 
        Expense = 2 
    }
}
```

### 2. Refactor the Category Model

Replace the entire contents of `Models/Category.cs`:

```csharp
namespace Budget_Buddy.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CategoryType Type { get; set; }
        public bool IsArchived { get; set; } = false;
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}
```

### 3. Update AppDbContext

Update your `AppDbContext.cs` to include the unique index:

```csharp
using Budget_Buddy.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasIndex(c => new { c.Name, c.Type })
            .IsUnique();
    }
}
```

### 4. Create and Apply Migration

Now we need to create a migration to update your database with the new schema. There are two ways to do this:

#### Option A: Using Visual Studio (Recommended for beginners)

1. **Open Package Manager Console**:
   - In Visual Studio, go to `Tools` → `NuGet Package Manager` → `Package Manager Console`
   - This will open a blue command window at the bottom of Visual Studio

2. **Create the Migration**:
   - In the Package Manager Console, type this command and press Enter:
   ```
   Add-Migration CreateCategories
   ```
   - Wait for the command to complete. You should see a message like "Build succeeded" and "Done"

3. **Apply the Migration**:
   - In the same Package Manager Console, type this command and press Enter:
   ```
   Update-Database
   ```
   - Wait for the command to complete. You should see a message like "Done"

#### Option B: Using Command Line (Alternative)

1. **Open Command Prompt or Terminal**:
   - Press `Windows + R`, type `cmd`, and press Enter (Windows)
   - Or open Terminal on Mac/Linux

2. **Navigate to Your Project**:
   - Use `cd` commands to navigate to your project folder
   - Example: `cd "C:\Users\YourName\Documents\Budget-Buddy\Budget-Buddy"`

3. **Create the Migration**:
   ```
   dotnet ef migrations add CreateCategories
   ```

4. **Apply the Migration**:
   ```
   dotnet ef database update
   ```

### 5. Verify the Migration

After running the migration commands:

1. **Check the Migrations Folder**:
   - In Visual Studio, look in your `Migrations` folder
   - You should see a new file with a name like `20240101_CreateCategories.cs`
   - This file contains the SQL commands that will update your database

2. **Check the Database**:
   - Open your `app.db` file with a SQLite browser
   - **Need help with SQLite?** See our [SQLite Setup and Usage Guide](../general/sqlite-setup-and-usage.md)
   - You should see the updated `Categories` table with the new columns:
     - `Id` (as GUID/text)
     - `Name` (text)
     - `Type` (integer - stores the enum value)
     - `IsArchived` (integer - 0 for false, 1 for true)
     - `CreatedUtc` (text - stores the datetime)

3. **Test Your Application**:
   - Run your application (`F5` or `Ctrl+F5` in Visual Studio)
   - Make sure it starts without errors
   - Check that the Swagger UI loads correctly

## Understanding the Changes

### Why These Changes Matter

1. **GUID instead of int**: Better for distributed systems and security
2. **CategoryType enum**: Type safety prevents invalid values
3. **Unique constraint**: Prevents duplicate categories of the same type
4. **IsArchived flag**: Soft delete instead of hard delete
5. **CreatedUtc**: Audit trail for when records were created

### Database Schema Changes

The migration will:
- Change `Id` from `int` to `GUID`
- Change `Type` from `string` to `int` (enum value)
- Add `IsArchived` boolean column
- Add `CreatedUtc` datetime column
- Create unique index on `(Name, Type)`

## Verification Steps

1. **Check the migration file**: Open the generated migration file to verify it contains the correct changes
2. **Run the application**: Start your API to ensure it still works
3. **Check the database**: Use a database browser to verify the new schema

## Common Issues and Solutions

### Issue: "Add-Migration command not found" or "Entity Framework tools not installed"
**Solution**: 
1. In Visual Studio, go to `Tools` → `NuGet Package Manager` → `Package Manager Console`
2. Run this command: `Install-Package Microsoft.EntityFrameworkCore.Tools`
3. Wait for it to install, then try `Add-Migration CreateCategories` again

### Issue: Migration conflicts or "database already exists"
**Solution**: 
1. Delete the `app.db` file from your project folder
2. Run `Update-Database` again
3. This will create a fresh database with the new schema

### Issue: "Enum not recognized" or compilation errors
**Solution**: 
1. Make sure you've added `using Budget_Buddy.Models;` at the top of your `AppDbContext.cs`
2. Check that your `CategoryType.cs` file is in the `Models` folder
3. Build your project (`Ctrl+Shift+B`) to check for compilation errors

### Issue: Migration fails with "table already exists"
**Solution**: 
1. In Package Manager Console, run: `Remove-Migration`
2. Delete the `app.db` file
3. Run `Add-Migration CreateCategories` again
4. Run `Update-Database`

### Issue: "Cannot find the project" when running commands
**Solution**: 
1. Make sure you're in the correct project folder
2. In Visual Studio, right-click on your project in Solution Explorer
3. Select "Open in File Explorer" to see the exact path
4. Use that path in your command line

### Issue: Application won't start after migration
**Solution**: 
1. Check the Output window in Visual Studio for error messages
2. Make sure all your code changes are saved (`Ctrl+S`)
3. Try building the project first (`Ctrl+Shift+B`)
4. If there are build errors, fix them before running the application

### Issue: Database file is locked
**Solution**: 
1. Stop your application if it's running
2. Close any database browser tools that might have the file open
3. Try the migration commands again

## Next Steps

After completing this step:
- Your Category model will be properly structured
- Your database will have the correct schema
- You'll be ready to create the CategoryController in S1.2

## What We're Learning

This refactoring teaches us:
- How to improve initial designs based on better requirements
- The importance of proper data modeling
- How to use migrations to evolve database schemas
- The value of constraints and validation at the database level
