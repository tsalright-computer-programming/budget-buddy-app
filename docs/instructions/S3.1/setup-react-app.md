# S3.1 - Setup React App with Vite and Axios

## Overview
In this step, you'll create a React frontend application that will communicate with your Budget Buddy API. This is a big step as you're learning a completely new technology (React) and new tools (Node.js, npm, Vite). Don't worry - we'll go through everything step by step!

## What You'll Learn
- How to set up a React application using Vite
- Understanding Node.js and npm (Node Package Manager)
- How to configure environment variables
- How to set up Axios for API communication
- Basic command line skills with Git
- Using Visual Studio Code for frontend development

## Prerequisites
- Your Budget Buddy API should be working (S0.1-S2.3)
- Basic understanding of HTML, CSS, and JavaScript (we'll explain as we go)

## Step-by-Step Instructions

### 1. Install Node.js (Required for React)

#### What is Node.js?
Node.js is like .NET for JavaScript - it lets you run JavaScript outside of a web browser. React needs Node.js to work.

#### Download and Install Node.js
1. **Open your web browser** and go to https://nodejs.org/
2. **Download the LTS version** (Long Term Support - the green button)
3. **Run the installer** and follow the installation wizard
4. **Accept all default settings** and click "Next" through each step
5. **Click "Finish" when done**

#### Verify Installation
1. **Open Command Prompt** (Windows) or Terminal (Mac)
2. **Type**: `node --version`
3. **Press Enter**
4. **You should see a version number** like `v20.x.x`
5. **Type**: `npm --version`
6. **Press Enter**
7. **You should see a version number** like `9.x.x`

### 2. Install Visual Studio Code

#### What is Visual Studio Code?
VS Code is a free code editor made by Microsoft. It's perfect for React development and has great features for JavaScript/TypeScript.

#### Download and Install VS Code
1. **Go to**: https://code.visualstudio.com/
2. **Click "Download for Windows"** (or Mac if you're on Mac)
3. **Run the installer** and follow the installation wizard
4. **Accept all default settings**
5. **Click "Finish" when done**

#### Install Useful Extensions
1. **Open VS Code**
2. **Click the Extensions icon** (square icon on the left sidebar)
3. **Search for and install these extensions**:
   - **ES7+ React/Redux/React-Native snippets** (by dsznajder)
   - **TypeScript Importer** (by pmneo)
   - **Auto Rename Tag** (by Jun Han)
   - **Bracket Pair Colorizer** (by CoenraadS)

### 3. Open Your Project in VS Code

1. **Open VS Code**
2. **Click "File" → "Open Folder"**
3. **Navigate to your Budget Buddy project folder**
4. **Select the entire project folder** (the one containing Budget-Buddy and docs)
5. **Click "Select Folder"**

You should now see your project structure in the left sidebar of VS Code.

### 4. Create the React App

#### What is Vite?
Vite is a build tool that makes React development fast and easy. It's like having a development server that automatically updates when you change your code.

#### Create the React App
1. **Open the Terminal in VS Code**:
   - Go to `Terminal` → `New Terminal`
   - Or press `Ctrl+`` (backtick) on Windows
   - Or press `Cmd+`` on Mac

2. **Navigate to your project root** (if not already there):
   ```bash
   cd /path/to/your/budget-buddy-app
   ```

3. **Create the React app**:
   ```bash
   npm create vite@latest finance-web -- --template react-ts
   ```
   - This creates a new folder called `finance-web`
   - The `--template react-ts` tells it to use React with TypeScript

4. **Navigate into the new folder**:
   ```bash
   cd finance-web
   ```

5. **Install the dependencies**:
   ```bash
   npm install
   ```
   - This downloads all the packages React needs to work

6. **Install additional packages we need**:
   ```bash
   npm install axios react-router-dom
   ```
   - `axios`: For making API calls to your backend
   - `react-router-dom`: For navigation between pages

### 5. Understanding the Project Structure

After creating the app, you should see this structure:

```
finance-web/
├── public/
│   └── vite.svg
├── src/
│   ├── App.css
│   ├── App.tsx
│   ├── index.css
│   ├── main.tsx
│   └── vite-env.d.ts
├── index.html
├── package.json
├── tsconfig.json
└── vite.config.ts
```

#### Key Files Explained:
- **`src/App.tsx`**: The main React component (like your Program.cs)
- **`src/main.tsx`**: The entry point (like your Program.cs)
- **`package.json`**: Lists all the packages your app needs
- **`vite.config.ts`**: Configuration for Vite
- **`index.html`**: The HTML file that loads your React app

### 6. Create Environment Configuration

#### What are Environment Variables?
Environment variables store configuration settings that can change between different environments (development, production, etc.).

1. **Create a new file** in the `finance-web` folder:
   - Right-click in the VS Code file explorer
   - Select "New File"
   - Name it `.env`

2. **Add this content** to the `.env` file:
   ```env
   VITE_API_BASE_URL=https://localhost:5001
   ```
   - This tells your React app where to find your API
   - The `VITE_` prefix is required for Vite to recognize the variable

### 7. Create the API Client

1. **Create a new file** in the `src` folder:
   - Right-click on the `src` folder
   - Select "New File"
   - Name it `api.ts`

2. **Add this content** to the `api.ts` file:
   ```typescript
   import axios from 'axios';

   export const api = axios.create({
     baseURL: import.meta.env.VITE_API_BASE_URL
   });
   ```

#### Understanding the API Client:
- **`axios`**: A library for making HTTP requests
- **`axios.create()`**: Creates a configured instance of axios
- **`baseURL`**: The base URL for all API calls
- **`import.meta.env.VITE_API_BASE_URL`**: Gets the environment variable we set

### 8. Test Your React App

1. **Start the development server**:
   ```bash
   npm run dev
   ```

2. **You should see output like**:
   ```
   VITE v5.0.0  ready in 500 ms

   ➜  Local:   http://localhost:5173/
   ➜  Network: use --host to expose
   ```

3. **Open your browser** and go to `http://localhost:5173/`

4. **You should see the Vite + React welcome page**

### 9. Test API Connection

Let's modify the default React app to test our API connection:

1. **Open `src/App.tsx`** in VS Code

2. **Replace the entire contents** with this code:
   ```typescript
   import { useState, useEffect } from 'react';
   import { api } from './api';
   import './App.css';

   function App() {
     const [apiStatus, setApiStatus] = useState<string>('Testing...');
     const [categories, setCategories] = useState<any[]>([]);

     useEffect(() => {
       // Test API connection
       const testApi = async () => {
         try {
           const response = await api.get('/api/categories');
           setApiStatus('✅ API Connected!');
           setCategories(response.data);
         } catch (error) {
           setApiStatus('❌ API Connection Failed');
           console.error('API Error:', error);
         }
       };

       testApi();
     }, []);

     return (
       <div className="App">
         <h1>Budget Buddy - React Frontend</h1>
         <p>Status: {apiStatus}</p>
         <p>Categories found: {categories.length}</p>
         <div>
           <h2>Categories:</h2>
           <ul>
             {categories.map((category) => (
               <li key={category.id}>
                 {category.name} ({category.type === 1 ? 'Income' : 'Expense'})
               </li>
             ))}
           </ul>
         </div>
       </div>
     );
   }

   export default App;
   ```

3. **Save the file** (`Ctrl+S`)

4. **Check your browser** - it should automatically refresh and show:
   - "✅ API Connected!" if your API is running
   - "❌ API Connection Failed" if your API is not running

### 10. Make Sure Your API is Running

If you see "API Connection Failed":

1. **Go back to Visual Studio** (where your API is)
2. **Run your API** (`F5` or `Ctrl+F5`)
3. **Check that it's running** at `https://localhost:5001` (or whatever port it shows)
4. **Go back to your browser** and refresh the page

### 11. Understanding What Just Happened

#### React Concepts:
- **Components**: Functions that return JSX (HTML-like code)
- **State**: Data that can change and causes the UI to update
- **useEffect**: Runs code when the component loads
- **JSX**: A way to write HTML inside JavaScript

#### API Communication:
- **axios.get()**: Makes a GET request to your API
- **async/await**: Handles asynchronous operations
- **Error handling**: Catches and displays errors

### 12. Git Setup and Workflow

#### What is Git?
Git is a version control system that tracks changes to your code. It's like "Save As" but much more powerful. Since you're working on a team project, Git will help you collaborate and keep track of your changes.

#### When to Create Branches and PRs
- **Create a branch** for each user story (S3.1, S3.2, S3.3)
- **Create a branch** for any bug fixes or new features
- **Create a PR** when you complete a user story or feature
- **Never work directly on the main branch** - always use feature branches

#### Recommended Workflow for This Project
1. **Create a branch** for S3.1: `git checkout -b s3.1/react-app-setup`
2. **Make your changes** and commit them
3. **Push your branch** to GitHub
4. **Create a PR** when S3.1 is complete
5. **Repeat** for S3.2 and S3.3

#### Initialize Git in Your React App:
1. **Open Terminal in VS Code**
2. **Navigate to your React app folder**:
   ```bash
   cd finance-web
   ```

3. **Initialize Git**:
   ```bash
   git init
   ```

4. **Add all files**:
   ```bash
   git add .
   ```

5. **Make your first commit**:
   ```bash
   git commit -m "Initial React app setup"
   ```

#### For Complete Git Workflow
**Need help with Git?** See our comprehensive [Git Workflow Guide](../general/git-workflow-guide.md) which covers:
- Creating branches and PRs
- Committing and pushing code
- Linking issues to PRs
- Using both Visual Studio and command line
- Best practices for team collaboration

## Common Issues and Solutions

### Issue: "npm is not recognized"
**Solution**: 
1. Restart your computer after installing Node.js
2. Make sure Node.js installed correctly
3. Try opening a new Command Prompt

### Issue: "Port 5173 is already in use"
**Solution**:
1. Close any other React apps that might be running
2. Or use a different port: `npm run dev -- --port 3000`

### Issue: "API Connection Failed"
**Solution**:
1. Make sure your API is running in Visual Studio
2. Check the port number in your `.env` file
3. Make sure there are no CORS issues (S0.3 should have fixed this)

### Issue: "Cannot find module 'axios'"
**Solution**:
1. Make sure you're in the `finance-web` folder
2. Run `npm install` again
3. Check that `axios` is in your `package.json`

### Issue: VS Code doesn't recognize TypeScript
**Solution**:
1. Install the TypeScript extension in VS Code
2. Restart VS Code
3. Make sure you're in the `finance-web` folder

## What You've Accomplished

By completing this step, you've learned:
- ✅ **How to set up a React application with Vite**
- ✅ **Understanding Node.js and npm**
- ✅ **How to configure environment variables**
- ✅ **How to set up Axios for API communication**
- ✅ **Basic command line skills**
- ✅ **Using Visual Studio Code for frontend development**
- ✅ **Basic Git commands**

## Next Steps

After completing this step:
- Your React app will be running and connected to your API
- You'll be ready to build the Categories page in S3.2
- You'll learn how to create forms and handle user input
- You'll understand how React components work

## Key Concepts Learned

### React Basics
- Components are functions that return JSX
- State manages data that can change
- useEffect runs code when components load
- JSX lets you write HTML in JavaScript

### Development Tools
- Node.js runs JavaScript outside the browser
- npm manages packages and dependencies
- Vite provides fast development experience
- VS Code is a powerful code editor

### API Communication
- Axios makes HTTP requests easy
- Environment variables store configuration
- Error handling prevents crashes
- Async/await handles asynchronous operations

**Congratulations!** 🎉 You've successfully set up a React application and connected it to your API. You're now ready to build the user interface for your Budget Buddy application!
