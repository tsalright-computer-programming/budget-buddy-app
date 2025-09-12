# Tools Installation Guide

## Overview

This guide will walk you through installing all the necessary tools to work on the Budget Buddy project. Don't worry if you've never installed software before - we'll go through each step carefully.

## What You'll Install

1. **Visual Studio 2022 Community** - The main program for writing code
2. **.NET 9.0 SDK** - The framework that runs your application
3. **Git** - A tool for managing code versions
4. **SQLite Browser** (Optional) - A tool to view your database

---

## Step 1: Install Visual Studio 2022 Community

### What is Visual Studio?
Visual Studio is like Microsoft Word, but for writing code instead of documents. It's free and has everything you need to build applications.

### Download and Install:
1. **Open your web browser** (Chrome, Edge, Firefox, etc.)
2. **Go to**: https://visualstudio.microsoft.com/vs/community/
3. **Click the big blue "Download" button**
4. **Wait for the download to finish** (this might take 10-15 minutes)
5. **Run the downloaded file** (it will be called something like `vs_community.exe`)
6. **Click "Yes"** when Windows asks for permission

### Visual Studio Installer Setup:
1. **When the installer opens**, you'll see a list of "workloads"
2. **Look for ".NET desktop development"** and check the box next to it
3. **Also check "ASP.NET and web development"** if you see it
4. **Click "Install"** in the bottom right corner
5. **Wait for installation** (this will take 20-30 minutes)
6. **Click "Restart"** when it's done

### Verify Installation:
1. **Click the Windows Start button**
2. **Type "Visual Studio"**
3. **Click on "Visual Studio 2022"** when it appears
4. **If it opens successfully, you're good to go!**

---

## Step 2: Install .NET 9.0 SDK

### What is .NET SDK?
.NET SDK is like the engine that runs your application. It's usually included with Visual Studio, but let's make sure you have the right version.

### Check if it's already installed:
1. **Open Command Prompt** (see Step 1 in the database setup guide)
2. **Type**: `dotnet --version`
3. **Press Enter**
4. **If you see a version number** (like 9.0.xxx), you're all set!
5. **If you see an error**, continue to the installation steps below

### Install .NET 9.0 SDK:
1. **Go to**: https://dotnet.microsoft.com/download/dotnet/9.0
2. **Click "Download .NET 9.0 SDK"** (the big blue button)
3. **Wait for the download to finish**
4. **Run the downloaded file** (it will be called something like `dotnet-sdk-9.0.xxx.exe`)
5. **Click "Yes"** when Windows asks for permission
6. **Follow the installation wizard** (just click "Next" on each screen)
7. **Click "Finish"** when done

### Verify Installation:
1. **Close and reopen Command Prompt**
2. **Type**: `dotnet --version`
3. **Press Enter**
4. **You should see**: `9.0.xxx` (some version number)

---

## Step 3: Install Git (Optional but Recommended)

### What is Git?
Git helps you save different versions of your code and share it with others. It's like "Save As" but much more powerful.

### Download and Install:
1. **Go to**: https://git-scm.com/download/win
2. **Click "Download for Windows"**
3. **Wait for the download to finish**
4. **Run the downloaded file** (it will be called something like `Git-2.xx.x-64-bit.exe`)
5. **Click "Yes"** when Windows asks for permission
6. **Follow the installation wizard** (the default settings are fine)
7. **Click "Next" on each screen until you reach "Finish"**

### Verify Installation:
1. **Open Command Prompt**
2. **Type**: `git --version`
3. **Press Enter**
4. **You should see**: `git version 2.xx.x`

---

## Step 4: Install SQLite Browser (Optional)

### What is SQLite Browser?
This tool lets you look at your database in a nice, visual way - like opening an Excel file.

### Download and Install:
1. **Go to**: https://sqlitebrowser.org/dl/
2. **Click "Download"** next to the Windows version
3. **Wait for the download to finish**
4. **Run the downloaded file** (it will be called something like `DB.Browser.for.SQLite-3.12.2-win64.msi`)
5. **Click "Yes"** when Windows asks for permission
6. **Follow the installation wizard** (default settings are fine)
7. **Click "Finish"** when done

### How to Use:
1. **Open SQLite Browser** from your Start menu
2. **Click "Open Database"**
3. **Navigate to your project folder** and find `app.db`
4. **Click "Open"**
5. **You can now see your database tables!**

---

## Troubleshooting

### Problem: "Command not recognized" after installing .NET
**Solution:**
1. **Close all Command Prompt windows**
2. **Restart your computer**
3. **Try the command again**

### Problem: Visual Studio won't open
**Solution:**
1. **Right-click on Visual Studio** in the Start menu
2. **Select "Run as administrator"**
3. **Click "Yes"** when Windows asks for permission

### Problem: Downloads are very slow
**Solution:**
1. **Try downloading during off-peak hours** (early morning or late evening)
2. **Make sure you have a stable internet connection**
3. **Close other programs** that might be using the internet

### Problem: Installation fails
**Solution:**
1. **Make sure you have enough disk space** (at least 5GB free)
2. **Close all other programs** before installing
3. **Run the installer as administrator** (right-click → "Run as administrator")

---

## What You've Accomplished

After completing this guide, you now have:
- ✅ **Visual Studio 2022 Community** - Your code editor
- ✅ **.NET 9.0 SDK** - The framework for your application
- ✅ **Git** - Version control (optional but helpful)
- ✅ **SQLite Browser** - Database viewer (optional)

## Next Steps

1. **Follow the [Database Setup Guide](setup-db-connections.md)** to get your project running
2. **Start coding** in Visual Studio
3. **Ask questions** if you get stuck!

## Getting Help

If you run into any problems:
1. **Take a screenshot** of any error messages
2. **Write down** exactly what you were doing when the problem happened
3. **Note** which step you were on
4. **Ask for help** with these details

---

**Congratulations!** 🎉 You now have all the tools needed to start building your Budget Buddy application. The hardest part (installing everything) is done!
