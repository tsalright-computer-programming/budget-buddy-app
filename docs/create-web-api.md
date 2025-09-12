# Creating a Basic ASP.NET Web API in Visual Studio 2022

## Overview

This guide will teach you how to create a brand new ASP.NET Web API project from scratch using Visual Studio 2022. You'll learn the fundamentals of web API development and how to test your endpoints.

## What You'll Learn

- How to create a new Web API project
- Understanding the project structure
- Creating your first API controller
- Adding Swagger UI for testing
- Testing endpoints in the browser
- Basic debugging techniques

---

## Step 1: Create a New Project

### Opening Visual Studio
1. **Click the Windows Start button**
2. **Type "Visual Studio"**
3. **Click on "Visual Studio 2022"** when it appears
4. **Wait for Visual Studio to open** (this might take a minute)

### Creating the Project
1. **On the start screen**, click "Create a new project"
2. **In the search box**, type "ASP.NET Core Web API"
3. **Click on "ASP.NET Core Web API"** (it should be the first result)
4. **Click "Next"**

### Project Configuration
1. **Project name**: Type "MyFirstWebAPI" (or any name you like)
2. **Location**: Choose where to save it (Desktop is fine)
3. **Solution name**: Leave it the same as project name
4. **Click "Next"**

### Additional Information
1. **Framework**: Make sure ".NET 9.0" is selected
2. **Authentication**: Leave as "None"
3. **Configure for HTTPS**: Check this box ✓
4. **Enable OpenAPI support**: Check this box ✓ (this adds Swagger)
5. **Use controllers**: Check this box ✓
6. **Click "Create"**

**What just happened?** Visual Studio created a new project with all the basic files you need for a Web API.

---

## Step 2: Understanding the Project Structure

### What You'll See
After the project loads, you'll see a "Solution Explorer" on the right side showing your project files:

```
MyFirstWebAPI/
├── Controllers/
│   └── WeatherForecastController.cs    # Example controller
├── Program.cs                          # Main application file
├── appsettings.json                   # Configuration settings
├── appsettings.Development.json       # Development settings
├── Properties/
│   └── launchSettings.json            # How the app runs
└── MyFirstWebAPI.csproj               # Project file
```

### Key Files Explained
- **Program.cs**: This is where your application starts and gets configured
- **Controllers/**: This folder contains your API endpoints
- **appsettings.json**: Configuration settings for your app
- **launchSettings.json**: Tells Visual Studio how to run your app

---

## Step 3: Run Your First API

### Starting the Application
1. **Press F5** on your keyboard, OR
2. **Click the green "Play" button** (▶️) at the top of Visual Studio
3. **Wait for the build to complete** (you'll see messages in the bottom panel)
4. **Your web browser should open automatically** showing the Swagger UI

### What You Should See
- A webpage with the title "Swagger UI"
- A section called "WeatherForecast" with a "GET" button
- This is the interactive documentation for your API

### If the Browser Doesn't Open
1. **Look at the bottom of Visual Studio** for a message like "Now listening on: https://localhost:7xxx"
2. **Copy that URL** (it will be something like https://localhost:7001)
3. **Open your web browser** and paste the URL
4. **Add "/swagger" to the end**: https://localhost:7001/swagger

---

## Step 4: Test the Default Endpoint

### Using Swagger UI
1. **On the Swagger page**, find the "WeatherForecast" section
2. **Click the "GET" button** (it should be blue)
3. **Click "Try it out"**
4. **Click "Execute"**
5. **Scroll down** to see the response

### What You Should See
You'll see a JSON response like this:
```json
[
  {
    "date": "2025-09-12T10:30:00.000Z",
    "temperatureC": 25,
    "temperatureF": 76,
    "summary": "Warm"
  },
  {
    "date": "2025-09-13T10:30:00.000Z",
    "temperatureC": 20,
    "temperatureF": 67,
    "summary": "Cool"
  }
]
```

**Congratulations!** Your API is working! 🎉

---

## Step 5: Create Your Own Health Endpoint

### Creating a New Controller
1. **Right-click on the "Controllers" folder** in Solution Explorer
2. **Select "Add" → "Controller"**
3. **Choose "API Controller - Empty"**
4. **Name it**: "HealthController"
5. **Click "Add"**

### Writing the Health Endpoint Code
Replace all the code in the HealthController.cs file with this:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = "ok", message = "API is running!" });
        }
    }
}
```

### Understanding the Code
- `[ApiController]`: Tells ASP.NET this is an API controller
- `[Route("[controller]")]`: Sets the URL path (will be /health)
- `[HttpGet]`: This method responds to GET requests
- `Ok()`: Returns a successful response with data

---

## Step 6: Test Your New Health Endpoint

### Method 1: Using Swagger UI
1. **Save your file** (Ctrl+S)
2. **Stop the application** (click the red square button or press Shift+F5)
3. **Run it again** (F5)
4. **Refresh the Swagger page** in your browser
5. **Look for "Health" section** with a GET button
6. **Click "Try it out" → "Execute"**

### Method 2: Direct URL Testing
1. **In your browser**, go to: `https://localhost:7xxx/health`
2. **Replace 7xxx** with your actual port number
3. **You should see**: `{"status":"ok","message":"API is running!"}`

---

## Step 7: Understanding Swagger UI

### What is Swagger?
Swagger is like a user manual for your API. It shows:
- All available endpoints
- What data each endpoint expects
- What data each endpoint returns
- A way to test endpoints without writing code

### Swagger Features
- **Interactive Testing**: Click buttons to test your API
- **Documentation**: Automatically generates docs from your code
- **Request/Response Examples**: Shows exactly what to send and expect

### Using Swagger Effectively
1. **Always test new endpoints** in Swagger first
2. **Check the response** to make sure it's what you expected
3. **Use it to show others** how your API works
4. **Reference it** when building frontend applications

---

## Step 8: Debugging Your API

### Setting Breakpoints
1. **Click in the left margin** next to a line of code (a red dot will appear)
2. **Run your application** (F5)
3. **Test the endpoint** that hits the breakpoint
4. **Visual Studio will pause** at the breakpoint
5. **You can inspect variables** and step through code

### Using the Output Window
1. **Go to View → Output**
2. **Select "Debug"** from the dropdown
3. **You'll see messages** about what your application is doing

### Common Debugging Tips
- **Check the Output window** for error messages
- **Use breakpoints** to see what's happening step by step
- **Test one endpoint at a time**
- **Read error messages carefully** - they usually tell you what's wrong

---

## Step 9: Adding More Endpoints

### Creating a Simple Data Endpoint
Add this method to your HealthController:

```csharp
[HttpGet("info")]
public IActionResult GetInfo()
{
    return Ok(new 
    { 
        application = "My First Web API",
        version = "1.0.0",
        timestamp = DateTime.UtcNow,
        environment = "Development"
    });
}
```

### Test the New Endpoint
1. **Save the file** (Ctrl+S)
2. **The application should restart automatically**
3. **In Swagger**, look for the new "GET /health/info" endpoint
4. **Test it** using the same process as before

---

## Troubleshooting

### Problem: "This site can't be reached"
**Solution:**
1. **Check if the application is running** (look for the URL in Visual Studio)
2. **Make sure you're using the correct port number**
3. **Try using HTTP instead of HTTPS**: `http://localhost:5xxx/health`

### Problem: Swagger page is blank
**Solution:**
1. **Check the Output window** for error messages
2. **Make sure you have "Enable OpenAPI support" checked** in project settings
3. **Try refreshing the browser page**

### Problem: Changes don't appear
**Solution:**
1. **Save the file** (Ctrl+S)
2. **Wait for the application to restart** (you'll see messages in the Output window)
3. **Refresh the browser page**

### Problem: "Controller not found"
**Solution:**
1. **Make sure your controller class name ends with "Controller"**
2. **Check that the namespace matches your project name**
3. **Build the project** (Build → Build Solution)

---

## What You've Accomplished

By following this guide, you've learned:
- ✅ **How to create a new Web API project** in Visual Studio
- ✅ **Understanding project structure** and key files
- ✅ **Creating API controllers** and endpoints
- ✅ **Using Swagger UI** for testing and documentation
- ✅ **Basic debugging techniques** and troubleshooting
- ✅ **Testing endpoints** in multiple ways

## Next Steps

Now that you have a working Web API, you can:
1. **Add more endpoints** for different functionality
2. **Connect to a database** (see the database setup guide)
3. **Add data validation** and error handling
4. **Deploy your API** to the cloud
5. **Build a frontend** that uses your API

## Getting Help

If you run into problems:
1. **Check the Output window** for error messages
2. **Use breakpoints** to debug step by step
3. **Test one thing at a time**
4. **Ask for help** with specific error messages

---

**Congratulations!** 🎉 You've successfully created your first ASP.NET Web API and learned the fundamentals of API development. You're now ready to build more complex applications!
