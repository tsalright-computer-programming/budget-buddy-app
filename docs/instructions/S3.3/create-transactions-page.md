# S3.3 - Create Transactions Page with Monthly Summary

## Overview
In this step, you'll build a comprehensive Transactions page that allows users to create, view, and filter transactions, plus see monthly financial summaries. This builds on everything you've learned so far and adds more advanced features like date filtering, currency formatting, and data aggregation.

## What You'll Learn
- How to build complex forms with multiple input types
- How to implement date filtering and range selection
- How to format currency values properly
- How to work with dropdowns and dynamic data
- How to display aggregated data and summaries
- How to handle complex state management

## Prerequisites
- S3.1 and S3.2 completed
- Your API should be running with all endpoints working
- Understanding of React components, state, and forms
- **Git workflow knowledge** - see our [Git Workflow Guide](../general/git-workflow-guide.md)

## Git Workflow Recommendations

### Before Starting S3.3
1. **Create a new branch** for this user story:
   ```bash
   git checkout -b s3.3/transactions-page
   ```

2. **Make sure you're on the correct branch** before making changes

### During Development
- **Commit frequently** as you complete each feature (helpers, form, table, summary, etc.)
- **Use descriptive commit messages** like "Add currency formatting helpers"
- **Test thoroughly** before committing

### When S3.3 is Complete
1. **Push your branch** to GitHub
2. **Create a Pull Request** with title "S3.3: Add Transactions Page with Monthly Summary"
3. **Link any related issues** in the PR description
4. **Request review** if working with a team
5. **This completes the full Budget Buddy application!** 🎉

## Step-by-Step Instructions

### 1. Create Helper Functions

First, let's create utility functions for handling currency conversion and formatting.

1. **Create a new file** called `utils.ts` in the `src` folder:
   - Right-click on `src` in VS Code
   - Select "New File"
   - Name it `utils.ts`

2. **Add this code** to `utils.ts`:
   ```typescript
   // Convert dollar string to cents (integer)
   export const toCents = (dollarString: string): number => {
     // Remove all non-numeric characters except decimal point
     const cleaned = dollarString.replace(/[^0-9.]/g, "");
     // Convert to number and multiply by 100, then round
     return Math.round(Number(cleaned) * 100);
   };

   // Convert cents to formatted dollar string
   export const formatDollars = (cents: number): string => {
     const isNegative = cents < 0;
     const absoluteCents = Math.abs(cents);
     const dollars = (absoluteCents / 100).toFixed(2);
     return (isNegative ? '-' : '') + '$' + dollars;
   };

   // Format date for display (YYYY-MM-DD to readable format)
   export const formatDate = (dateString: string): string => {
     const date = new Date(dateString);
     return date.toLocaleDateString('en-US', {
       year: 'numeric',
       month: 'short',
       day: 'numeric'
     });
   };

   // Get date 30 days ago for default filter
   export const getDefaultFromDate = (): string => {
     const date = new Date();
     date.setDate(date.getDate() - 30);
     return date.toISOString().split('T')[0];
   };

   // Get today's date for default filter
   export const getDefaultToDate = (): string => {
     return new Date().toISOString().split('T')[0];
   };
   ```

### 2. Create the Transactions Page

1. **Create a new file** called `TransactionsPage.tsx` in the `pages` folder:
   - Right-click on the `pages` folder
   - Select "New File"
   - Name it `TransactionsPage.tsx`

2. **Add this code** to `TransactionsPage.tsx`:
   ```typescript
   import { useState, useEffect } from 'react';
   import { api } from '../api';
   import { toCents, formatDollars, formatDate, getDefaultFromDate, getDefaultToDate } from '../utils';

   interface Transaction {
     id: string;
     postedDate: string;
     description: string;
     amountCents: number;
     categoryId: string | null;
     categoryName: string | null;
     categoryType: number | null;
   }

   interface Category {
     id: string;
     name: string;
     type: number;
     isArchived: boolean;
   }

   interface MonthlySummary {
     month: string;
     incomeCents: number;
     expenseCents: number;
     netCents: number;
   }

   interface TransactionFormData {
     postedDate: string;
     description: string;
     amountDollars: string;
     categoryId: string;
   }

   export default function TransactionsPage() {
     const [transactions, setTransactions] = useState<Transaction[]>([]);
     const [categories, setCategories] = useState<Category[]>([]);
     const [monthlySummary, setMonthlySummary] = useState<MonthlySummary | null>(null);
     const [loading, setLoading] = useState(true);
     const [error, setError] = useState<string>('');
     const [success, setSuccess] = useState<string>('');
     const [editingTransaction, setEditingTransaction] = useState<Transaction | null>(null);
     
     // Filter states
     const [fromDate, setFromDate] = useState(getDefaultFromDate());
     const [toDate, setToDate] = useState(getDefaultToDate());
     const [selectedCategoryId, setSelectedCategoryId] = useState<string>('');
     const [selectedType, setSelectedType] = useState<string>('');
     
     // Form state
     const [formData, setFormData] = useState<TransactionFormData>({
       postedDate: new Date().toISOString().split('T')[0],
       description: '',
       amountDollars: '',
       categoryId: ''
     });

     // Load data when component mounts
     useEffect(() => {
       loadCategories();
       loadTransactions();
       loadMonthlySummary();
     }, []);

     // Load transactions when filters change
     useEffect(() => {
       loadTransactions();
     }, [fromDate, toDate, selectedCategoryId, selectedType]);

     const loadCategories = async () => {
       try {
         const response = await api.get('/api/categories?includeArchived=false');
         setCategories(response.data);
       } catch (err) {
         console.error('Error loading categories:', err);
       }
     };

     const loadTransactions = async () => {
       try {
         setLoading(true);
         setError('');
         
         const params = new URLSearchParams();
         if (fromDate) params.append('from', fromDate);
         if (toDate) params.append('to', toDate);
         if (selectedCategoryId) params.append('categoryId', selectedCategoryId);
         if (selectedType) params.append('type', selectedType);
         
         const response = await api.get(`/api/transactions?${params.toString()}`);
         setTransactions(response.data);
       } catch (err) {
         setError('Failed to load transactions');
         console.error('Error loading transactions:', err);
       } finally {
         setLoading(false);
       }
     };

     const loadMonthlySummary = async () => {
       try {
         const currentMonth = new Date().toISOString().slice(0, 7); // YYYY-MM format
         const response = await api.get(`/api/summary?month=${currentMonth}`);
         setMonthlySummary(response.data);
       } catch (err) {
         console.error('Error loading monthly summary:', err);
       }
     };

     const handleSubmit = async (e: React.FormEvent) => {
       e.preventDefault();
       setError('');
       setSuccess('');

       try {
         const transactionData = {
           postedDate: formData.postedDate,
           description: formData.description,
           amountCents: toCents(formData.amountDollars),
           categoryId: formData.categoryId
         };

         if (editingTransaction) {
           // Update existing transaction
           await api.put(`/api/transactions/${editingTransaction.id}`, transactionData);
           setSuccess('Transaction updated successfully!');
         } else {
           // Create new transaction
           await api.post('/api/transactions', transactionData);
           setSuccess('Transaction created successfully!');
         }
         
         // Reset form and reload data
         setFormData({
           postedDate: new Date().toISOString().split('T')[0],
           description: '',
           amountDollars: '',
           categoryId: ''
         });
         setEditingTransaction(null);
         loadTransactions();
         loadMonthlySummary();
       } catch (err: any) {
         setError(err.response?.data?.detail || 'Failed to save transaction');
       }
     };

     const handleEdit = (transaction: Transaction) => {
       setEditingTransaction(transaction);
       setFormData({
         postedDate: transaction.postedDate,
         description: transaction.description,
         amountDollars: formatDollars(transaction.amountCents).replace('$', ''),
         categoryId: transaction.categoryId || ''
       });
     };

     const handleCancel = () => {
       setEditingTransaction(null);
       setFormData({
         postedDate: new Date().toISOString().split('T')[0],
         description: '',
         amountDollars: '',
         categoryId: ''
       });
     };

     const handleDelete = async (transaction: Transaction) => {
       if (window.confirm('Are you sure you want to delete this transaction?')) {
         try {
           await api.delete(`/api/transactions/${transaction.id}`);
           setSuccess('Transaction deleted successfully!');
           loadTransactions();
           loadMonthlySummary();
         } catch (err: any) {
           setError(err.response?.data?.detail || 'Failed to delete transaction');
         }
       }
     };

     const handleFilterChange = () => {
       loadTransactions();
     };

     const getTransactionSign = (categoryType: number | null): string => {
       return categoryType === 1 ? '+' : '-';
     };

     const getTransactionColor = (categoryType: number | null): string => {
       return categoryType === 1 ? '#28a745' : '#dc3545';
     };

     if (loading && transactions.length === 0) {
       return <div>Loading transactions...</div>;
     }

     return (
       <div>
         <h2>Transactions Management</h2>
         
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

         {/* Monthly Summary */}
         {monthlySummary && (
           <div style={{ 
             border: '1px solid #ccc', 
             padding: '20px', 
             borderRadius: '4px',
             marginBottom: '30px',
             backgroundColor: '#f8f9fa'
           }}>
             <h3>Monthly Summary ({monthlySummary.month})</h3>
             <div style={{ display: 'flex', gap: '20px', flexWrap: 'wrap' }}>
               <div style={{ textAlign: 'center' }}>
                 <div style={{ fontSize: '24px', fontWeight: 'bold', color: '#28a745' }}>
                   {formatDollars(monthlySummary.incomeCents)}
                 </div>
                 <div>Income</div>
               </div>
               <div style={{ textAlign: 'center' }}>
                 <div style={{ fontSize: '24px', fontWeight: 'bold', color: '#dc3545' }}>
                   {formatDollars(monthlySummary.expenseCents)}
                 </div>
                 <div>Expenses</div>
               </div>
               <div style={{ textAlign: 'center' }}>
                 <div style={{ 
                   fontSize: '24px', 
                   fontWeight: 'bold', 
                   color: monthlySummary.netCents >= 0 ? '#28a745' : '#dc3545'
                 }}>
                   {formatDollars(monthlySummary.netCents)}
                 </div>
                 <div>Net</div>
               </div>
             </div>
           </div>
         )}

         {/* Filters */}
         <div style={{ 
           border: '1px solid #ccc', 
           padding: '20px', 
           borderRadius: '4px',
           marginBottom: '30px',
           backgroundColor: '#f9f9f9'
         }}>
           <h3>Filters</h3>
           <div style={{ display: 'flex', gap: '15px', flexWrap: 'wrap', alignItems: 'end' }}>
             <div>
               <label>
                 From Date:
                 <input
                   type="date"
                   value={fromDate}
                   onChange={(e) => setFromDate(e.target.value)}
                   style={{ marginLeft: '5px', padding: '5px' }}
                 />
               </label>
             </div>
             <div>
               <label>
                 To Date:
                 <input
                   type="date"
                   value={toDate}
                   onChange={(e) => setToDate(e.target.value)}
                   style={{ marginLeft: '5px', padding: '5px' }}
                 />
               </label>
             </div>
             <div>
               <label>
                 Category:
                 <select
                   value={selectedCategoryId}
                   onChange={(e) => setSelectedCategoryId(e.target.value)}
                   style={{ marginLeft: '5px', padding: '5px' }}
                 >
                   <option value="">All Categories</option>
                   {categories.map((category) => (
                     <option key={category.id} value={category.id}>
                       {category.name} ({category.type === 1 ? 'Income' : 'Expense'})
                     </option>
                   ))}
                 </select>
               </label>
             </div>
             <div>
               <label>
                 Type:
                 <select
                   value={selectedType}
                   onChange={(e) => setSelectedType(e.target.value)}
                   style={{ marginLeft: '5px', padding: '5px' }}
                 >
                   <option value="">All Types</option>
                   <option value="1">Income</option>
                   <option value="2">Expense</option>
                 </select>
               </label>
             </div>
             <div>
               <button
                 onClick={handleFilterChange}
                 style={{
                   padding: '8px 16px',
                   backgroundColor: '#007bff',
                   color: 'white',
                   border: 'none',
                   borderRadius: '4px',
                   cursor: 'pointer'
                 }}
               >
                 Apply Filters
               </button>
             </div>
           </div>
         </div>

         {/* Transaction Form */}
         <div style={{ 
           border: '1px solid #ccc', 
           padding: '20px', 
           borderRadius: '4px',
           marginBottom: '30px',
           backgroundColor: '#f9f9f9'
         }}>
           <h3>{editingTransaction ? 'Edit Transaction' : 'Create New Transaction'}</h3>
           <form onSubmit={handleSubmit}>
             <div style={{ display: 'flex', gap: '15px', flexWrap: 'wrap', marginBottom: '15px' }}>
               <div>
                 <label>
                   Date:
                   <input
                     type="date"
                     value={formData.postedDate}
                     onChange={(e) => setFormData({ ...formData, postedDate: e.target.value })}
                     required
                     style={{ marginLeft: '5px', padding: '5px' }}
                   />
                 </label>
               </div>
               <div>
                 <label>
                   Description:
                   <input
                     type="text"
                     value={formData.description}
                     onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                     required
                     placeholder="Enter description"
                     style={{ marginLeft: '5px', padding: '5px', width: '200px' }}
                   />
                 </label>
               </div>
               <div>
                 <label>
                   Amount ($):
                   <input
                     type="text"
                     value={formData.amountDollars}
                     onChange={(e) => setFormData({ ...formData, amountDollars: e.target.value })}
                     required
                     placeholder="0.00"
                     style={{ marginLeft: '5px', padding: '5px', width: '100px' }}
                   />
                 </label>
               </div>
               <div>
                 <label>
                   Category:
                   <select
                     value={formData.categoryId}
                     onChange={(e) => setFormData({ ...formData, categoryId: e.target.value })}
                     required
                     style={{ marginLeft: '5px', padding: '5px' }}
                   >
                     <option value="">Select Category</option>
                     {categories.map((category) => (
                       <option key={category.id} value={category.id}>
                         {category.name} ({category.type === 1 ? 'Income' : 'Expense'})
                       </option>
                     ))}
                   </select>
                 </label>
               </div>
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
                 {editingTransaction ? 'Update' : 'Create'}
               </button>
               
               {editingTransaction && (
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

         {/* Transactions Table */}
         <div>
           <h3>Transactions ({transactions.length})</h3>
           {transactions.length === 0 ? (
             <p>No transactions found for the selected filters.</p>
           ) : (
             <table style={{ 
               width: '100%', 
               borderCollapse: 'collapse',
               border: '1px solid #ccc'
             }}>
               <thead>
                 <tr style={{ backgroundColor: '#f8f9fa' }}>
                   <th style={{ padding: '10px', border: '1px solid #ccc', textAlign: 'left' }}>Date</th>
                   <th style={{ padding: '10px', border: '1px solid #ccc', textAlign: 'left' }}>Description</th>
                   <th style={{ padding: '10px', border: '1px solid #ccc', textAlign: 'left' }}>Category</th>
                   <th style={{ padding: '10px', border: '1px solid #ccc', textAlign: 'right' }}>Amount</th>
                   <th style={{ padding: '10px', border: '1px solid #ccc', textAlign: 'left' }}>Actions</th>
                 </tr>
               </thead>
               <tbody>
                 {transactions.map((transaction) => (
                   <tr key={transaction.id}>
                     <td style={{ padding: '10px', border: '1px solid #ccc' }}>
                       {formatDate(transaction.postedDate)}
                     </td>
                     <td style={{ padding: '10px', border: '1px solid #ccc' }}>
                       {transaction.description}
                     </td>
                     <td style={{ padding: '10px', border: '1px solid #ccc' }}>
                       {transaction.categoryName || 'No Category'}
                     </td>
                     <td style={{ 
                       padding: '10px', 
                       border: '1px solid #ccc', 
                       textAlign: 'right',
                       color: getTransactionColor(transaction.categoryType),
                       fontWeight: 'bold'
                     }}>
                       {getTransactionSign(transaction.categoryType)}{formatDollars(transaction.amountCents)}
                     </td>
                     <td style={{ padding: '10px', border: '1px solid #ccc' }}>
                       <button
                         onClick={() => handleEdit(transaction)}
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
                       <button
                         onClick={() => handleDelete(transaction)}
                         style={{
                           padding: '4px 8px',
                           backgroundColor: '#dc3545',
                           color: 'white',
                           border: 'none',
                           borderRadius: '4px',
                           cursor: 'pointer'
                         }}
                       >
                         Delete
                       </button>
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

### 3. Update the App.tsx to Include the Transactions Route

1. **Open `src/App.tsx`** and update the Routes section:
   ```typescript
   import { Routes, Route, Link } from 'react-router-dom';
   import CategoriesPage from './pages/CategoriesPage';
   import TransactionsPage from './pages/TransactionsPage';
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
             <Route path="/transactions" element={<TransactionsPage />} />
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

### 4. Test Your Transactions Page

1. **Make sure your API is running** in Visual Studio
2. **Make sure your React app is running** (`npm run dev`)
3. **Open your browser** and go to `http://localhost:5173`
4. **Click on "Transactions"** in the navigation
5. **You should see the Transactions page**

### 5. Test the Features

#### Create a Transaction:
1. **Select a date** (defaults to today)
2. **Enter a description** like "Grocery shopping"
3. **Enter an amount** like "25.50"
4. **Select a category** from the dropdown
5. **Click "Create"**
6. **You should see the transaction** appear in the table

#### Test Filtering:
1. **Change the date range** using the From/To date inputs
2. **Select a specific category** from the dropdown
3. **Select a type** (Income or Expense)
4. **Click "Apply Filters"**
5. **You should see only matching transactions**

#### Test the Monthly Summary:
1. **Create some income transactions** (positive amounts with Income categories)
2. **Create some expense transactions** (positive amounts with Expense categories)
3. **The monthly summary should update** to show totals

## Common Issues and Solutions

### Issue: "Cannot find module '../utils'"
**Solution**: Make sure the `utils.ts` file is in the `src` folder and the import path is correct.

### Issue: "Categories not loading in dropdown"
**Solution**: 
1. Make sure your API is running
2. Check that the categories endpoint is working
3. Verify the API URL in your `.env` file

### Issue: "Amount formatting not working"
**Solution**: 
1. Check the `toCents` and `formatDollars` functions
2. Make sure the input is a valid number
3. Check the browser console for errors

### Issue: "Date filtering not working"
**Solution**: 
1. Make sure the date format is correct (YYYY-MM-DD)
2. Check that the API is receiving the correct parameters
3. Verify the date range is valid

### Issue: "Monthly summary not updating"
**Solution**: 
1. Make sure you have transactions in the current month
2. Check that the summary API endpoint is working
3. Verify the month format is correct

## What You've Accomplished

By completing this step, you've learned:
- ✅ **How to build complex forms with multiple input types**
- ✅ **How to implement date filtering and range selection**
- ✅ **How to format currency values properly**
- ✅ **How to work with dropdowns and dynamic data**
- ✅ **How to display aggregated data and summaries**
- ✅ **How to handle complex state management**
- ✅ **How to build a complete financial tracking application**

## Next Steps

After completing this step:
- You'll have a fully functional Budget Buddy application
- You'll understand how to build complex React applications
- You'll know how to integrate with APIs effectively
- You'll be ready to add more advanced features

## Key Concepts Learned

### Complex State Management
- Multiple useState hooks for different data
- useEffect for side effects and data loading
- State updates trigger re-renders
- Complex state can be managed with multiple useState hooks

### Form Handling
- Multiple input types (text, date, select, number)
- Controlled inputs update state
- Form validation and error handling
- Form state can be reset after successful submission

### Data Filtering and Display
- Date range filtering
- Category and type filtering
- Dynamic data loading based on filters
- Organized data display with tables

### Currency and Date Formatting
- Converting between dollars and cents
- Formatting currency for display
- Date formatting and parsing
- Handling different date formats

**Congratulations!** 🎉 You've successfully created a comprehensive Transactions page with monthly summaries. You now have a complete Budget Buddy application with both backend API and frontend UI!

## What You've Built

Your complete Budget Buddy application now includes:
- ✅ **Backend API** with Categories, Transactions, and Summary endpoints
- ✅ **Frontend React App** with full CRUD operations
- ✅ **Categories Management** with create, edit, and archive functionality
- ✅ **Transactions Management** with filtering and date range selection
- ✅ **Monthly Financial Summaries** with income, expense, and net totals
- ✅ **Responsive UI** with forms, tables, and navigation
- ✅ **Error Handling** and user feedback
- ✅ **Data Validation** and currency formatting

You've built a complete personal finance application from scratch!
