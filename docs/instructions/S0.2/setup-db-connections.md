# Database Setup Instructions for Beginners

## What You'll Need
- Windows computer
- Visual Studio 2022 Community Edition (already installed)
- .NET 9.0 SDK (should be included with Visual Studio)

## Step 1: Opening the Command Prompt (Terminal)

### How to Open Command Prompt on Windows:
1. **Method 1 - Using Start Menu:**
   - Click the Windows Start button (bottom left corner)
   - Type "cmd" or "command prompt"
   - Click on "Command Prompt" when it appears

2. **Method 2 - Using Run Dialog:**
   - Press `Windows key + R` on your keyboard
   - Type "cmd" and press Enter

3. **Method 3 - Right-click Method:**
   - Right-click on the Start button
   - Select "Windows Terminal" or "Command Prompt"

### What You'll See:
- A black window will open with text like: `C:\Users\YourName>`
- This is called the "command prompt" or "terminal"
- Don't worry - you can't break anything by typing in here!

## Step 2: Finding Your Project Folder

### Navigate to Your Project:
1. In the command prompt, type: `cd` (then press Space)
2. Now you need to find your project folder. Look for a folder called "Budget-Buddy" on your computer
3. **To find the folder path:**
   - Open File Explorer (click the folder icon in your taskbar)
   - Navigate to where you saved the project
   - Right-click on the "Budget-Buddy" folder
   - Select "Copy as path"
   - Go back to the command prompt and right-click to paste the path
   - Press Enter

**Example:** If your project is on the Desktop, you might type:
```
cd C:\Users\YourName\Desktop\budget-buddy-app\Budget-Buddy
```

### Verify You're in the Right Place:
Type `dir` and press Enter. You should see folders like "Budget-Buddy" and files like "Budget-Buddy.sln"

## Step 3: Restore Project Packages

### What This Does:
This downloads all the code libraries your project needs to work.

### How to Do It:
1. In the command prompt, type: `dotnet restore`
2. Press Enter
3. Wait for it to finish (you'll see lots of text scrolling by)
4. When it's done, you'll see your command prompt again

**If you get an error:** Make sure you're in the right folder (the one with Budget-Buddy.sln file)

## Step 4: Install Database Tools

### What This Does:
This installs special tools that help create and manage your database.

### How to Do It:
1. Type: `dotnet tool install --global dotnet-ef`
2. Press Enter
3. Wait for it to finish (this might take a minute or two)
4. You should see a message saying "Tool 'dotnet-ef' was successfully installed"

**Note:** If you get a warning about PATH, don't worry - we'll handle that in the next step.

## Step 5: Create the Database

### Step 5a: Create Database Structure (Migration)
1. Type: `dotnet ef migrations add InitialCreate --project Budget-Buddy\Budget-Buddy.csproj`
2. Press Enter
3. Wait for it to finish
4. You should see "Done. To undo this action, use 'ef migrations remove'"

### Step 5b: Apply Migration to Create Database
1. Type: `dotnet ef database update --project Budget-Buddy\Budget-Buddy.csproj`
2. Press Enter
3. Wait for it to finish
4. You should see messages about creating tables

**What Just Happened:**
- A file called "app.db" was created in your project folder
- This is your database file (like an Excel file, but for programs)
- Tables were created to store your data
- **Want to explore your database?** See our [SQLite Setup and Usage Guide](../general/sqlite-setup-and-usage.md)

## Step 6: Run the Application

### How to Start Your Application:
1. Type: `dotnet run --project Budget-Buddy\Budget-Buddy.csproj`
2. Press Enter
3. Wait for it to start (you'll see messages about building and starting)
4. When it's ready, you'll see: "Now listening on: http://localhost:5044"

**Important:** Keep this command prompt window open! Closing it will stop your application.

## Step 7: Test Your Application

### Open Your Web Browser:
1. Open any web browser (Chrome, Edge, Firefox, etc.)
2. In the address bar, type: `http://localhost:5044/health`
3. Press Enter
4. You should see: `{"status":"ok"}`

### Test the Database Connection:
1. In the address bar, type: `http://localhost:5044/budget`
2. Press Enter
3. You should see something like:
```json
{
  "status": "ok",
  "database": "connected",
  "budgetItemsCount": 0,
  "timestamp": "2025-09-12T03:59:15.547Z"
}
```

### View the API Documentation:
1. In the address bar, type: `http://localhost:5044/swagger`
2. Press Enter
3. You'll see a nice webpage showing all your API endpoints

## Troubleshooting - Common Problems and Solutions

### Problem 1: "Command not recognized" or "dotnet is not recognized"
**What this means:** Windows doesn't know where to find the dotnet program.

**Solution:**
1. Close the command prompt
2. Open Visual Studio 2022
3. Go to Tools → Get Tools and Features
4. Make sure ".NET desktop development" workload is installed
5. If not installed, check the box and click "Modify"
6. Restart your computer
7. Try the command prompt again

### Problem 2: "Could not find project file"
**What this means:** You're not in the right folder.

**Solution:**
1. In the command prompt, type `dir` and press Enter
2. Look for a file called "Budget-Buddy.sln"
3. If you don't see it, you need to navigate to the correct folder
4. Use the folder navigation steps from Step 2 above

### Problem 3: "Port 5044 is already in use"
**What this means:** Another program is using the same port.

**Solution:**
1. Close the command prompt window that's running your application
2. Wait 10 seconds
3. Try running the application again
4. If it still doesn't work, restart your computer

### Problem 4: "No such table: Categories"
**What this means:** The database wasn't created properly.

**Solution:**
1. Make sure you completed Step 5 (Create the Database)
2. If you skipped it, go back and do both Step 5a and 5b
3. If you already did it, try this:
   - Type: `dotnet ef database update --project Budget-Buddy\Budget-Buddy.csproj`
   - Press Enter

### Problem 5: "Access denied" or "Permission denied"
**What this means:** Windows is blocking the operation.

**Solution:**
1. Close the command prompt
2. Right-click on the Start button
3. Select "Windows PowerShell (Admin)" or "Command Prompt (Admin)"
4. Click "Yes" when Windows asks for permission
5. Try the commands again

### Problem 6: The application starts but the website doesn't work
**What this means:** The application is running but there might be a network issue.

**Solution:**
1. Make sure you typed the URL exactly: `http://localhost:5044/health`
2. Try a different web browser
3. Check if your antivirus is blocking the connection
4. Try `http://127.0.0.1:5044/health` instead

## What Each Part Does (Simple Explanation)

### The Database (app.db file)
- Think of it like an Excel spreadsheet, but for programs
- It stores your data in organized tables
- The "Categories" table stores different types of budget categories

### The API Endpoints
- **/health** - A simple check to see if your program is running
- **/budget** - Tests if the database is working and shows how many categories you have
- **/swagger** - A fancy webpage that shows you all the available features

### The Command Prompt
- It's like a text-based way to control your computer
- Instead of clicking buttons, you type commands
- It's how programmers talk to their programs

## Getting Help

If you get stuck:
1. **Take a screenshot** of any error messages
2. **Write down** exactly what you typed
3. **Note** which step you were on when the problem happened
4. **Ask for help** with these details

## Success! What You've Accomplished

When everything is working, you've successfully:
- ✅ Set up a web API (a program that can talk to websites)
- ✅ Connected it to a database (a place to store information)
- ✅ Created a way to test if everything is working
- ✅ Learned basic command line skills
- ✅ Built a foundation for a budget tracking application

**Congratulations!** You're now ready to start building more features for your budget application.
