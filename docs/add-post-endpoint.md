# Adding a POST Endpoint to Create Categories

## Overview

This guide will teach you how to add a POST endpoint to your Budget controller that allows creating new categories. You'll learn about HTTP methods, data validation, and how to save data to the database.

## What You'll Learn

- How to create POST endpoints
- Data validation and error handling
- Saving data to the database
- Testing POST requests with Swagger UI
- Understanding HTTP status codes

---

## Step 1: Understanding POST Endpoints

### What is a POST Endpoint?
- **GET**: Retrieves data (like reading a book)
- **POST**: Creates new data (like writing in a new book)
- **PUT**: Updates existing data (like editing a book)
- **DELETE**: Removes data (like throwing away a book)

### When to Use POST
- Creating new records in the database
- Submitting forms
- Adding new items to a list
- User registration

---

## Step 2: Examine the Current Budget Controller

### Open the Budget Controller
1. **In Visual Studio**, open `Controllers/BudgetController.cs`
2. **Look at the current code** - you should see a GET endpoint that counts categories

### Current Structure
```csharp
[HttpGet]
public async Task<IActionResult> Get()
{
    // ... existing code that counts categories
}
```

---

## Step 3: Add the POST Endpoint

### Add the POST Method
Add this code to your BudgetController class (after the existing GET method):

```csharp
[HttpPost]
public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
{
    try
    {
        // Validate the request
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { error = "Category name is required" });
        }

        if (string.IsNullOrWhiteSpace(request.Type))
        {
            return BadRequest(new { error = "Category type is required" });
        }

        // Create the new category
        var category = new Category
        {
            Name = request.Name.Trim(),
            Type = request.Type.Trim()
        };

        // Add to database
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        // Return success response
        return CreatedAtAction(nameof(Get), new { id = category.Id }, new
        {
            id = category.Id,
            name = category.Name,
            type = category.Type,
            message = "Category created successfully"
        });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { error = "An error occurred while creating the category" });
    }
}
```

### Understanding the Code
- `[HttpPost]`: This method responds to POST requests
- `[FromBody]`: The data comes from the request body (JSON)
- `CreateCategoryRequest`: A class we'll create to define the expected data
- `BadRequest()`: Returns a 400 status code for validation errors
- `CreatedAtAction()`: Returns a 201 status code for successful creation

---

## Step 4: Create the Request Model

### Create a New File
1. **Right-click on the "Models" folder** in Solution Explorer
2. **Select "Add" → "Class"**
3. **Name it**: "CreateCategoryRequest"
4. **Click "Add"**

### Add the Request Model Code
Replace all the code in the new file with this:

```csharp
namespace Budget_Buddy.Models
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
```

### Understanding the Model
- This defines what data the POST endpoint expects
- `Name` and `Type` are the required fields
- `= string.Empty` provides default values

---

## Step 5: Update the Budget Controller Imports

### Add the Required Using Statement
At the top of your BudgetController.cs file, make sure you have these imports:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Budget_Buddy.Models;
```

### Complete Controller Structure
Your BudgetController should now look like this:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Budget_Buddy.Models;

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
            // ... existing GET method code
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            // ... new POST method code
        }
    }
}
```

---

## Step 6: Test Your POST Endpoint

### Using Swagger UI
1. **Save all files** (Ctrl+Shift+S)
2. **Run your application** (F5)
3. **Go to the Swagger page** in your browser
4. **Look for the "Budget" section**
5. **You should now see two endpoints**:
   - GET /budget (existing)
   - POST /budget (new)

### Testing the POST Endpoint
1. **Click on the POST /budget endpoint**
2. **Click "Try it out"**
3. **In the request body box**, enter this JSON:
```json
{
  "name": "Groceries",
  "type": "Expense"
}
```
4. **Click "Execute"**
5. **Check the response** - you should see a 201 status code with the created category

### Testing with Different Data
Try these examples:

**Example 1 - Income Category:**
```json
{
  "name": "Salary",
  "type": "Income"
}
```

**Example 2 - Another Expense:**
```json
{
  "name": "Gas",
  "type": "Expense"
}
```

---

## Step 7: Understanding HTTP Status Codes

### Common Status Codes
- **200 OK**: Request successful
- **201 Created**: New resource created successfully
- **400 Bad Request**: Invalid data sent
- **500 Internal Server Error**: Server error occurred

### Your POST Endpoint Returns
- **201 Created**: When category is created successfully
- **400 Bad Request**: When required fields are missing
- **500 Internal Server Error**: When database error occurs

---

## Step 8: Testing Validation

### Test Missing Name
1. **In Swagger**, try this request body:
```json
{
  "name": "",
  "type": "Expense"
}
```
2. **You should get a 400 error** with message "Category name is required"

### Test Missing Type
1. **Try this request body:**
```json
{
  "name": "Test Category",
  "type": ""
}
```
2. **You should get a 400 error** with message "Category type is required"

### Test Empty Request
1. **Try this request body:**
```json
{}
```
2. **You should get a 400 error** for missing required fields

---

## Step 9: Verify Data in Database

### Check Your Categories
1. **Test the GET endpoint** again in Swagger
2. **You should see the count increased** by the number of categories you created
3. **The response should show**: `"budgetItemsCount": X` (where X is the number of categories)

### Using SQLite Browser (Optional)
1. **Open SQLite Browser** (if you installed it)
2. **Open your app.db file**
3. **Go to the "Browse Data" tab**
4. **Select "Categories" table**
5. **You should see your new categories** with their IDs, names, and types

---

## Step 10: Understanding the Complete Flow

### What Happens When You POST
1. **Client sends JSON data** to `/budget` endpoint
2. **ASP.NET deserializes** the JSON into `CreateCategoryRequest` object
3. **Validation checks** ensure required fields are present
4. **New Category object** is created with the data
5. **Entity Framework adds** the category to the database
6. **Database saves** the changes
7. **Success response** is returned with the new category data

### Error Handling
- **Validation errors** return 400 with specific error messages
- **Database errors** return 500 with generic error message
- **Try-catch block** prevents the application from crashing

---

## Troubleshooting

### Problem: "CreateCategoryRequest not found"
**Solution:**
1. **Make sure you created the CreateCategoryRequest.cs file** in the Models folder
2. **Check the namespace** matches your project name
3. **Build the solution** (Build → Build Solution)

### Problem: POST endpoint doesn't appear in Swagger
**Solution:**
1. **Make sure you saved all files**
2. **Stop and restart the application**
3. **Check for compilation errors** in the Error List window

### Problem: "Database error" when creating category
**Solution:**
1. **Make sure the database migration was applied** (see database setup guide)
2. **Check that the Categories table exists**
3. **Verify the connection string** in appsettings.json

### Problem: Validation not working
**Solution:**
1. **Check that you're sending JSON data** in the request body
2. **Make sure the Content-Type header** is set to "application/json"
3. **Verify the field names** match exactly (case-sensitive)

---

## What You've Accomplished

By following this guide, you've learned:
- ✅ **How to create POST endpoints** for data creation
- ✅ **Data validation techniques** and error handling
- ✅ **Request/response models** for API design
- ✅ **HTTP status codes** and their meanings
- ✅ **Testing POST requests** with Swagger UI
- ✅ **Database operations** with Entity Framework
- ✅ **Error handling** and troubleshooting

## Next Steps

Now that you can create categories, you can:
1. **Add PUT endpoints** to update existing categories
2. **Add DELETE endpoints** to remove categories
3. **Add more validation rules** (like unique names)
4. **Create endpoints for other entities** (like budget items)
5. **Add authentication** to secure your endpoints

## Advanced Features to Try

### Add More Validation
```csharp
if (request.Name.Length > 50)
{
    return BadRequest(new { error = "Category name must be 50 characters or less" });
}
```

### Add Duplicate Check
```csharp
var existingCategory = await _context.Categories
    .FirstOrDefaultAsync(c => c.Name.ToLower() == request.Name.ToLower());
    
if (existingCategory != null)
{
    return BadRequest(new { error = "Category with this name already exists" });
}
```

---

**Congratulations!** 🎉 You've successfully added a POST endpoint to create categories. You now understand the fundamentals of API data creation and are ready to build more complex features!
