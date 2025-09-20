# S0.3 - Enable CORS for React Development

## Overview
In this step, you'll configure Cross-Origin Resource Sharing (CORS) in your ASP.NET Core API to allow requests from your React development server running on `http://localhost:5173`.

## Why CORS is Needed
When your React app runs on `http://localhost:5173` and tries to call your API (which runs on a different port), browsers block these requests due to CORS policy. We need to explicitly allow these cross-origin requests during development.

## Step-by-Step Instructions

### 1. Open Program.cs
Navigate to your `Program.cs` file in the `Budget-Buddy/Budget-Buddy/` directory.

### 2. Add CORS Configuration
Add the following CORS configuration to your `Program.cs` file. You'll need to add this in two places:

#### A. Add CORS Service Registration
Find the section where services are being added (around line 10-15) and add the CORS service:

```csharp
// Add CORS service
const string DevCors = "DevCors";
builder.Services.AddCors(options => options.AddPolicy(DevCors, policy => policy
    .WithOrigins("http://localhost:5173")
    .AllowAnyHeader()
    .AllowAnyMethod()));
```

#### B. Add CORS Middleware
Find the section where middleware is being configured (around line 20-25) and add the CORS middleware **before** the `app.MapControllers()` line:

```csharp
// Use CORS
app.UseCors(DevCors);
```

### 3. Complete Program.cs Example
Your `Program.cs` should look something like this:

```csharp
using Microsoft.EntityFrameworkCore;
using Budget_Buddy.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS service
const string DevCors = "DevCors";
builder.Services.AddCors(options => options.AddPolicy(DevCors, policy => policy
    .WithOrigins("http://localhost:5173")
    .AllowAnyHeader()
    .AllowAnyMethod()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors(DevCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
```

### 4. Test Your Configuration
1. **Start your API**: Run your ASP.NET Core API using `dotnet run` or through Visual Studio
2. **Verify the API is running**: Check that your API is accessible at `https://localhost:7xxx` (note the port number)
3. **Test CORS**: Later, when you create your React app, you'll be able to make requests from `http://localhost:5173` to your API without CORS errors

### 5. Understanding the CORS Configuration

Let's break down what each part does:

- **`const string DevCors = "DevCors"`**: Creates a named policy for easy reference
- **`WithOrigins("http://localhost:5173")`**: Allows requests specifically from the React dev server
- **`AllowAnyHeader()`**: Permits any headers in the request
- **`AllowAnyMethod()`**: Allows GET, POST, PUT, DELETE, and other HTTP methods
- **`app.UseCors(DevCors)`**: Applies the CORS policy to all requests

## Verification
Once you've completed these steps:
- Your API will accept requests from `http://localhost:5173`
- You won't see CORS errors in the browser when calling the API from React
- The configuration is specifically for development (you'll need different CORS settings for production)

## Next Steps
After completing this step, you'll be ready to create your React frontend that can communicate with your API without CORS issues.

## Troubleshooting
- **Make sure the CORS middleware is added before `app.MapControllers()`**
- **Double-check the origin URL matches exactly: `http://localhost:5173`**
- **If you're still getting CORS errors, check the browser's developer console for specific error messages**
