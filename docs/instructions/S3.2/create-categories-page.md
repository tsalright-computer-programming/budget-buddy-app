# S3.2 - Create Categories Page with CRUD Operations

## Overview
In this step, you'll build a complete Categories management page with the ability to list, create, edit, and archive categories. This builds on what you learned in S3.1 about React basics, but adds more complex features like forms, routing, and state management.

## What You'll Learn
- How to create React components and pages
- How to use React Router for navigation
- How to build forms with validation
- How to handle API calls and loading states
- How to manage complex state in React
- How to display error messages and success feedback

## Prerequisites
- S3.1 completed (React app setup)
- Your API should be running with Categories endpoints working
- Basic understanding of React components and JSX
- **Git workflow knowledge** - see our [Git Workflow Guide](../general/git-workflow-guide.md)

## Git Workflow Recommendations

### Before Starting S3.2
1. **Create a new branch** for this user story:
   ```bash
   git checkout -b s3.2/categories-page
   ```

2. **Make sure you're on the correct branch** before making changes

### During Development
- **Commit frequently** as you complete each feature (form, table, validation, etc.)
- **Use descriptive commit messages** like "Add Categories form with validation"
- **Test thoroughly** before committing

### When S3.2 is Complete
1. **Push your branch** to GitHub
2. **Create a Pull Request** with title "S3.2: Add Categories Page with CRUD Operations"
3. **Link any related issues** in the PR description
4. **Request review** if working with a team

## Step-by-Step Instructions

### 1. Set Up React Router

#### What is React Router?
React Router lets you create different "pages" in your single-page application. It's like having multiple HTML pages, but all in one React app.

#### Install and Configure React Router
1. **Open `src/main.tsx`** in VS Code
2. **Replace the entire contents** with this code:
   ```typescript
   import React from 'react'
   import ReactDOM from 'react-dom/client'
   import { BrowserRouter } from 'react-router-dom'
   import App from './App.tsx'
   import './index.css'

   ReactDOM.createRoot(document.getElementById('root')!).render(
     <React.StrictMode>
       <BrowserRouter>
         <App />
       </BrowserRouter>
     </React.StrictMode>,
   )
   ```

3. **Open `src/App.tsx`** and replace with this code:
   ```typescript
   import { Routes, Route, Link } from 'react-router-dom';
   import CategoriesPage from './pages/CategoriesPage';
   import './App.css';

   function App() {
     return (
       <div className="App">
         <nav style={{ padding: '20px', borderBottom: '1px solid #ccc' }}>
           <h1>Budget Buddy</h1>
           <div style={{ marginTop: '10px' }}>
             <Link to="/" style={{ marginRight: '20px', textDecoration: 'none' }}>
               Home
             </Link>
             <Link to="/categories" style={{ marginRight: '20px', textDecoration: 'none' }}>
               Categories
             </Link>
             <Link to="/transactions" style={{ textDecoration: 'none' }}>
               Transactions
             </Link>
           </div>
         </nav>
         
         <main style={{ padding: '20px' }}>
           <Routes>
             <Route path="/" element={<HomePage />} />
             <Route path="/categories" element={<CategoriesPage />} />
             <Route path="/transactions" element={<div>Transactions page coming soon!</div>} />
           </Routes>
         </main>
       </div>
     );
   }

   function HomePage() {
     return (
       <div>
         <h2>Welcome to Budget Buddy!</h2>
         <p>Manage your personal finances with ease.</p>
         <p>Use the navigation above to get started.</p>
       </div>
     );
   }

   export default App;
   ```

### 2. Create the Categories Page

1. **Create a new folder** called `pages` in the `src` folder:
   - Right-click on `src` in VS Code
   - Select "New Folder"
   - Name it `pages`

2. **Create a new file** called `CategoriesPage.tsx` in the `pages` folder:
   - Right-click on the `pages` folder
   - Select "New File"
   - Name it `CategoriesPage.tsx`

3. **Add this code** to `CategoriesPage.tsx`:
   ```typescript
   import { useState, useEffect } from 'react';
   import { api } from '../api';

   interface Category {
     id: string;
     name: string;
     type: number; // 1 = Income, 2 = Expense
     isArchived: boolean;
   }

   interface CategoryFormData {
     name: string;
     type: number;
   }

   export default function CategoriesPage() {
     const [categories, setCategories] = useState<Category[]>([]);
     const [loading, setLoading] = useState(true);
     const [error, setError] = useState<string>('');
     const [success, setSuccess] = useState<string>('');
     const [showArchived, setShowArchived] = useState(false);
     const [editingCategory, setEditingCategory] = useState<Category | null>(null);
     const [formData, setFormData] = useState<CategoryFormData>({
       name: '',
       type: 2 // Default to Expense
     });

     // Load categories when component mounts
     useEffect(() => {
       loadCategories();
     }, [showArchived]);

     const loadCategories = async () => {
       try {
         setLoading(true);
         setError('');
         const response = await api.get(`/api/categories?includeArchived=${showArchived}`);
         setCategories(response.data);
       } catch (err) {
         setError('Failed to load categories');
         console.error('Error loading categories:', err);
       } finally {
         setLoading(false);
       }
     };

     const handleSubmit = async (e: React.FormEvent) => {
       e.preventDefault();
       setError('');
       setSuccess('');

       try {
         if (editingCategory) {
           // Update existing category
           await api.put(`/api/categories/${editingCategory.id}`, {
             ...formData,
             isArchived: editingCategory.isArchived
           });
           setSuccess('Category updated successfully!');
         } else {
           // Create new category
           await api.post('/api/categories', formData);
           setSuccess('Category created successfully!');
         }
         
         // Reset form and reload categories
         setFormData({ name: '', type: 2 });
         setEditingCategory(null);
         loadCategories();
       } catch (err: any) {
         setError(err.response?.data?.detail || 'Failed to save category');
       }
     };

     const handleEdit = (category: Category) => {
       setEditingCategory(category);
       setFormData({
         name: category.name,
         type: category.type
       });
     };

     const handleCancel = () => {
       setEditingCategory(null);
       setFormData({ name: '', type: 2 });
     };

     const handleArchive = async (category: Category) => {
       try {
         await api.delete(`/api/categories/${category.id}`);
         setSuccess('Category archived successfully!');
         loadCategories();
       } catch (err: any) {
         setError(err.response?.data?.detail || 'Failed to archive category');
       }
     };

     const handleUnarchive = async (category: Category) => {
       try {
         await api.put(`/api/categories/${category.id}`, {
           name: category.name,
           type: category.type,
           isArchived: false
         });
         setSuccess('Category unarchived successfully!');
         loadCategories();
       } catch (err: any) {
         setError(err.response?.data?.detail || 'Failed to unarchive category');
       }
     };

     const incomeCategories = categories.filter(cat => cat.type === 1);
     const expenseCategories = categories.filter(cat => cat.type === 2);

     if (loading) {
       return <div>Loading categories...</div>;
     }

     return (
       <div>
         <h2>Categories Management</h2>
         
         {/* Success/Error Messages */}
         {success && (
           <div style={{ 
             padding: '10px', 
             backgroundColor: '#d4edda', 
             color: '#155724', 
             border: '1px solid #c3e6cb',
             borderRadius: '4px',
             marginBottom: '20px'
           }}>
             {success}
           </div>
         )}
         
         {error && (
           <div style={{ 
             padding: '10px', 
             backgroundColor: '#f8d7da', 
             color: '#721c24', 
             border: '1px solid #f5c6cb',
             borderRadius: '4px',
             marginBottom: '20px'
           }}>
             {error}
           </div>
         )}

         {/* Show/Hide Archived Toggle */}
         <div style={{ marginBottom: '20px' }}>
           <label>
             <input
               type="checkbox"
               checked={showArchived}
               onChange={(e) => setShowArchived(e.target.checked)}
             />
             Show archived categories
           </label>
         </div>

         {/* Category Form */}
         <div style={{ 
           border: '1px solid #ccc', 
           padding: '20px', 
           borderRadius: '4px',
           marginBottom: '30px',
           backgroundColor: '#f9f9f9'
         }}>
           <h3>{editingCategory ? 'Edit Category' : 'Create New Category'}</h3>
           <form onSubmit={handleSubmit}>
             <div style={{ marginBottom: '15px' }}>
               <label>
                 Name:
                 <input
                   type="text"
                   value={formData.name}
                   onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                   required
                   style={{ 
                     marginLeft: '10px', 
                     padding: '5px', 
                     width: '200px',
                     border: '1px solid #ccc',
                     borderRadius: '4px'
                   }}
                 />
               </label>
             </div>
             
             <div style={{ marginBottom: '15px' }}>
               <label>
                 Type:
                 <select
                   value={formData.type}
                   onChange={(e) => setFormData({ ...formData, type: parseInt(e.target.value) })}
                   style={{ 
                     marginLeft: '10px', 
                     padding: '5px',
                     border: '1px solid #ccc',
                     borderRadius: '4px'
                   }}
                 >
                   <option value={1}>Income</option>
                   <option value={2}>Expense</option>
                 </select>
               </label>
             </div>
             
             <div>
               <button 
                 type="submit"
                 style={{
                   padding: '8px 16px',
                   backgroundColor: '#007bff',
                   color: 'white',
                   border: 'none',
                   borderRadius: '4px',
                   cursor: 'pointer',
                   marginRight: '10px'
                 }}
               >
                 {editingCategory ? 'Update' : 'Create'}
               </button>
               
               {editingCategory && (
                 <button 
                   type="button"
                   onClick={handleCancel}
                   style={{
                     padding: '8px 16px',
                     backgroundColor: '#6c757d',
                     color: 'white',
                     border: 'none',
                     borderRadius: '4px',
                     cursor: 'pointer'
                   }}
                 >
                   Cancel
                 </button>
               )}
             </div>
           </form>
         </div>

         {/* Income Categories */}
         <div style={{ marginBottom: '30px' }}>
           <h3>Income Categories</h3>
           {incomeCategories.length === 0 ? (
             <p>No income categories found.</p>
           ) : (
             <table style={{ 
               width: '100%', 
               borderCollapse: 'collapse',
               border: '1px solid #ccc'
             }}>
               <thead>
                 <tr style={{ backgroundColor: '#f8f9fa' }}>
                   <th style={{ padding: '10px', border: '1px solid #ccc', textAlign: 'left' }}>Name</th>
                   <th style={{ padding: '10px', border: '1px solid #ccc', textAlign: 'left' }}>Status</th>
                   <th style={{ padding: '10px', border: '1px solid #ccc', textAlign: 'left' }}>Actions</th>
                 </tr>
               </thead>
               <tbody>
                 {incomeCategories.map((category) => (
                   <tr key={category.id}>
                     <td style={{ padding: '10px', border: '1px solid #ccc' }}>{category.name}</td>
                     <td style={{ padding: '10px', border: '1px solid #ccc' }}>
                       {category.isArchived ? 'Archived' : 'Active'}
                     </td>
                     <td style={{ padding: '10px', border: '1px solid #ccc' }}>
                       <button
                         onClick={() => handleEdit(category)}
                         style={{
                           padding: '4px 8px',
                           backgroundColor: '#28a745',
                           color: 'white',
                           border: 'none',
                           borderRadius: '4px',
                           cursor: 'pointer',
                           marginRight: '5px'
                         }}
                       >
                         Edit
                       </button>
                       {category.isArchived ? (
                         <button
                           onClick={() => handleUnarchive(category)}
                           style={{
                             padding: '4px 8px',
                             backgroundColor: '#17a2b8',
                             color: 'white',
                             border: 'none',
                             borderRadius: '4px',
                             cursor: 'pointer'
                           }}
                         >
                           Unarchive
                         </button>
                       ) : (
                         <button
                           onClick={() => handleArchive(category)}
                           style={{
                             padding: '4px 8px',
                             backgroundColor: '#dc3545',
                             color: 'white',
                             border: 'none',
                             borderRadius: '4px',
                             cursor: 'pointer'
                           }}
                         >
                           Archive
                         </button>
                       )}
                     </td>
                   </tr>
                 ))}
               </tbody>
             </table>
           )}
         </div>

         {/* Expense Categories */}
         <div>
           <h3>Expense Categories</h3>
           {expenseCategories.length === 0 ? (
             <p>No expense categories found.</p>
           ) : (
             <table style={{ 
               width: '100%', 
               borderCollapse: 'collapse',
               border: '1px solid #ccc'
             }}>
               <thead>
                 <tr style={{ backgroundColor: '#f8f9fa' }}>
                   <th style={{ padding: '10px', border: '1px solid #ccc', textAlign: 'left' }}>Name</th>
                   <th style={{ padding: '10px', border: '1px solid #ccc', textAlign: 'left' }}>Status</th>
                   <th style={{ padding: '10px', border: '1px solid #ccc', textAlign: 'left' }}>Actions</th>
                 </tr>
               </thead>
               <tbody>
                 {expenseCategories.map((category) => (
                   <tr key={category.id}>
                     <td style={{ padding: '10px', border: '1px solid #ccc' }}>{category.name}</td>
                     <td style={{ padding: '10px', border: '1px solid #ccc' }}>
                       {category.isArchived ? 'Archived' : 'Active'}
                     </td>
                     <td style={{ padding: '10px', border: '1px solid #ccc' }}>
                       <button
                         onClick={() => handleEdit(category)}
                         style={{
                           padding: '4px 8px',
                           backgroundColor: '#28a745',
                           color: 'white',
                           border: 'none',
                           borderRadius: '4px',
                           cursor: 'pointer',
                           marginRight: '5px'
                         }}
                       >
                         Edit
                       </button>
                       {category.isArchived ? (
                         <button
                           onClick={() => handleUnarchive(category)}
                           style={{
                             padding: '4px 8px',
                             backgroundColor: '#17a2b8',
                             color: 'white',
                             border: 'none',
                             borderRadius: '4px',
                             cursor: 'pointer'
                           }}
                         >
                           Unarchive
                         </button>
                       ) : (
                         <button
                           onClick={() => handleArchive(category)}
                           style={{
                             padding: '4px 8px',
                             backgroundColor: '#dc3545',
                             color: 'white',
                             border: 'none',
                             borderRadius: '4px',
                             cursor: 'pointer'
                           }}
                         >
                           Archive
                         </button>
                       )}
                     </td>
                   </tr>
                 ))}
               </tbody>
             </table>
           )}
         </div>
       </div>
     );
   }
   ```

### 3. Test Your Categories Page

1. **Make sure your API is running** in Visual Studio
2. **Make sure your React app is running** (`npm run dev` in the finance-web folder)
3. **Open your browser** and go to `http://localhost:5173`
4. **Click on "Categories"** in the navigation
5. **You should see the Categories page**

### 4. Test the CRUD Operations

#### Create a Category:
1. **Enter a name** like "Groceries"
2. **Select "Expense"** from the type dropdown
3. **Click "Create"**
4. **You should see the category appear** in the Expense Categories table

#### Edit a Category:
1. **Click "Edit"** next to any category
2. **Change the name** or type
3. **Click "Update"**
4. **You should see the changes** reflected in the table

#### Archive a Category:
1. **Click "Archive"** next to any category
2. **The category should disappear** from the table
3. **Check "Show archived categories"** to see it again
4. **Click "Unarchive"** to restore it

### 5. Understanding the Code

#### React Concepts Used:
- **State Management**: `useState` for managing data that can change
- **Side Effects**: `useEffect` for loading data when component mounts
- **Event Handling**: Functions that respond to user interactions
- **Conditional Rendering**: Showing different content based on state
- **Form Handling**: Controlled inputs that update state

#### API Integration:
- **GET Request**: Loading categories from the API
- **POST Request**: Creating new categories
- **PUT Request**: Updating existing categories
- **DELETE Request**: Archiving categories
- **Error Handling**: Displaying API errors to the user

#### UI Features:
- **Loading States**: Showing "Loading..." while fetching data
- **Success/Error Messages**: Feedback for user actions
- **Form Validation**: Required fields and proper data types
- **Responsive Tables**: Organized display of data
- **Action Buttons**: Edit, Archive, Unarchive functionality

## Common Issues and Solutions

### Issue: "Cannot find module 'react-router-dom'"
**Solution**: Make sure you installed it: `npm install react-router-dom`

### Issue: "API Connection Failed"
**Solution**: 
1. Make sure your API is running
2. Check the port number in your `.env` file
3. Make sure CORS is configured (S0.3)

### Issue: "Categories not loading"
**Solution**:
1. Check the browser console for errors
2. Make sure your API endpoints are working
3. Verify the API URL in your `.env` file

### Issue: "Form not submitting"
**Solution**:
1. Make sure all required fields are filled
2. Check the browser console for validation errors
3. Verify the API endpoint URLs

### Issue: "Edit button not working"
**Solution**:
1. Make sure the `handleEdit` function is properly defined
2. Check that the category data is being passed correctly
3. Verify the form state is being updated

## What You've Accomplished

By completing this step, you've learned:
- ✅ **How to create React components and pages**
- ✅ **How to use React Router for navigation**
- ✅ **How to build forms with validation**
- ✅ **How to handle API calls and loading states**
- ✅ **How to manage complex state in React**
- ✅ **How to display error messages and success feedback**
- ✅ **How to implement CRUD operations in React**

## Next Steps

After completing this step:
- You'll have a fully functional Categories management page
- You'll be ready to build the Transactions page in S3.3
- You'll understand how to build complex React applications
- You'll know how to integrate with APIs effectively

## Key Concepts Learned

### React State Management
- useState for managing component state
- useEffect for side effects and data loading
- State updates trigger re-renders
- Complex state can be managed with multiple useState hooks

### Form Handling
- Controlled inputs update state
- Form submission prevents default behavior
- Validation can be done before submission
- Form state can be reset after successful submission

### API Integration
- Axios makes HTTP requests easy
- Error handling prevents crashes
- Loading states improve user experience
- Success feedback confirms actions

### User Experience
- Clear error messages help users understand problems
- Success messages confirm actions
- Loading states show progress
- Organized data display improves readability

**Congratulations!** 🎉 You've successfully created a comprehensive Categories management page with full CRUD operations. You're now ready to build the Transactions page in S3.3!
