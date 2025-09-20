# SQLite Setup and Usage Guide

## Overview
SQLite is a lightweight database that stores all data in a single file. In our Budget Buddy app, the database is stored as `app.db`. This guide will help you install a SQLite browser and understand how to work with your database.

## What is SQLite?
- **File-based database**: All data is stored in a single `.db` file
- **No server required**: Unlike MySQL or PostgreSQL, SQLite doesn't need a separate server
- **Perfect for development**: Great for learning and small applications
- **Cross-platform**: Works on Windows, Mac, and Linux

## Installing SQLite Browser

### Option 1: DB Browser for SQLite (Recommended)

#### Windows Installation
1. **Download DB Browser for SQLite**:
   - Go to https://sqlitebrowser.org/
   - Click "Download" on the main page
   - Choose the Windows installer (usually the first option)

2. **Install the software**:
   - Run the downloaded `.exe` file
   - Follow the installation wizard
   - Accept the default settings
   - Click "Install" and then "Finish"

3. **Verify installation**:
   - Look for "DB Browser for SQLite" in your Start menu
   - Click to open it

#### Mac Installation
1. **Download DB Browser for SQLite**:
   - Go to https://sqlitebrowser.org/
   - Click "Download" on the main page
   - Choose the Mac version (`.dmg` file)

2. **Install the software**:
   - Open the downloaded `.dmg` file
   - Drag "DB Browser for SQLite" to your Applications folder
   - Eject the disk image

3. **Verify installation**:
   - Open Applications folder
   - Double-click "DB Browser for SQLite" to open it

### Option 2: SQLiteStudio (Alternative)

#### Windows Installation
1. **Download SQLiteStudio**:
   - Go to https://sqlitestudio.pl/
   - Click "Download" → "Windows" → "Installer"
   - Download the `.exe` file

2. **Install**:
   - Run the installer
   - Follow the setup wizard
   - Choose installation location (default is fine)

#### Mac Installation
1. **Download SQLiteStudio**:
   - Go to https://sqlitestudio.pl/
   - Click "Download" → "macOS" → "DMG"
   - Download the `.dmg` file

2. **Install**:
   - Open the `.dmg` file
   - Drag SQLiteStudio to Applications folder
   - Open from Applications

## Opening Your Database

### Finding Your Database File
Your database file is located at:
```
Budget-Buddy/Budget-Buddy/app.db
```

### Opening in DB Browser for SQLite
1. **Open DB Browser for SQLite**
2. **Click "Open Database"** (folder icon in toolbar)
3. **Navigate to your project folder**:
   - Go to your Budget Buddy project folder
   - Navigate to `Budget-Buddy/Budget-Buddy/`
   - Select `app.db`
   - Click "Open"

### Opening in SQLiteStudio
1. **Open SQLiteStudio**
2. **Click "Add Database"** (plus icon)
3. **Choose "SQLite"**
4. **Browse to your database file**:
   - Navigate to `Budget-Buddy/Budget-Buddy/app.db`
   - Click "OK"
5. **Double-click the database** in the left panel to connect

## Understanding Your Database

### Database Structure
When you open your database, you'll see:

#### Tables Tab
- **Categories**: Your main table for budget categories
- **Columns**: Id, Name, Type, IsArchived, CreatedUtc

#### Data Tab
- **Browse table data**: Click on a table to see its contents
- **Add/Edit/Delete records**: Use the interface to modify data

#### Structure Tab
- **View table schema**: See column definitions and constraints
- **Indexes**: See unique constraints and indexes

### Reading Your Data

#### Viewing Categories
1. **Click on "Categories" table** in the left panel
2. **Click "Data" tab** to see all category records
3. **Scroll through the data** to see all categories

#### Understanding Column Values
- **Id**: Unique identifier (GUID format)
- **Name**: Category name (e.g., "Groceries", "Salary")
- **Type**: 1 = Income, 2 = Expense
- **IsArchived**: 0 = Active, 1 = Archived
- **CreatedUtc**: When the record was created

## Common Database Operations

### Adding Test Data
1. **Click "Data" tab**
2. **Click "New Record"** (plus icon)
3. **Fill in the fields**:
   - Id: Leave blank (auto-generated)
   - Name: Enter category name
   - Type: Enter 1 for Income, 2 for Expense
   - IsArchived: Enter 0 for active
   - CreatedUtc: Enter current date/time
4. **Click "Apply"** to save

### Editing Data
1. **Click on a record** in the data view
2. **Make your changes** in the fields
3. **Click "Apply"** to save changes

### Deleting Data
1. **Right-click on a record**
2. **Select "Delete Record"**
3. **Confirm deletion**

### Running SQL Queries
1. **Click "Execute SQL" tab**
2. **Type your SQL query**:
   ```sql
   SELECT * FROM Categories WHERE Type = 2;
   ```
3. **Click "Execute"** (play button)
4. **View results** in the bottom panel

## Useful SQL Queries for Your Project

### View All Categories
```sql
SELECT * FROM Categories;
```

### View Only Income Categories
```sql
SELECT * FROM Categories WHERE Type = 1;
```

### View Only Expense Categories
```sql
SELECT * FROM Categories WHERE Type = 2;
```

### View Active Categories Only
```sql
SELECT * FROM Categories WHERE IsArchived = 0;
```

### View Archived Categories
```sql
SELECT * FROM Categories WHERE IsArchived = 1;
```

### Count Categories by Type
```sql
SELECT Type, COUNT(*) as Count 
FROM Categories 
GROUP BY Type;
```

### Find Categories by Name
```sql
SELECT * FROM Categories WHERE Name LIKE '%Groceries%';
```

## Troubleshooting

### Database File Not Found
- **Check the file path**: Make sure you're looking in the right folder
- **Check file extension**: Should be `.db`, not `.db-journal` or `.db-wal`
- **Run your application first**: The database file is created when you first run the app

### Database is Locked
- **Close your application**: Stop the Budget Buddy app
- **Close other database tools**: Make sure no other programs have the file open
- **Wait a moment**: Sometimes there's a brief lock after the app closes

### Can't See Your Data
- **Check if you have data**: Run your app and create some categories first
- **Refresh the view**: Click the refresh button in your database browser
- **Check the correct table**: Make sure you're looking at the "Categories" table

### Database Browser Won't Open
- **Try a different browser**: If DB Browser doesn't work, try SQLiteStudio
- **Check file permissions**: Make sure you have read/write access to the file
- **Run as administrator**: On Windows, try right-clicking and "Run as administrator"

## Best Practices

### Before Making Changes
1. **Always backup your database**: Copy `app.db` to `app_backup.db`
2. **Close your application**: Stop the Budget Buddy app before making changes
3. **Test changes**: Make small changes and test them

### When Working with Data
1. **Use the interface**: Prefer the GUI over direct SQL when possible
2. **Be careful with deletes**: Remember that deletes are permanent
3. **Check constraints**: Make sure your data follows the rules (unique names, etc.)

### When Running Queries
1. **Start simple**: Begin with basic SELECT statements
2. **Test on copies**: Try queries on backup data first
3. **Understand the results**: Make sure you understand what the query returns

## Next Steps

Once you're comfortable with SQLite:
- **Learn more SQL**: Practice with different query types
- **Understand relationships**: Learn about foreign keys and joins
- **Explore advanced features**: Indexes, views, and triggers
- **Consider other databases**: PostgreSQL, MySQL for larger applications

## Getting Help

### Common Resources
- **SQLite Documentation**: https://www.sqlite.org/docs.html
- **SQL Tutorial**: https://www.w3schools.com/sql/
- **DB Browser Help**: Help menu in the application

### When You're Stuck
1. **Check the error message**: Read what the database browser tells you
2. **Try a simpler query**: Break down complex queries into smaller parts
3. **Ask for help**: Share the specific error or question you have
4. **Check your data**: Make sure the data you're working with is correct
