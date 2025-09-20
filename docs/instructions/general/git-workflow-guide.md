# Git Workflow Guide for Budget Buddy App

## Overview
This guide covers the complete Git workflow for the Budget Buddy application, including creating branches, committing code, creating pull requests, and linking issues. You'll learn both Visual Studio and command line approaches to work with the [Budget Buddy repository](https://github.com/tsalright-computer-programming/budget-buddy-app).

## What You'll Learn
- How to create and manage Git branches
- How to commit and push code changes
- How to create pull requests on GitHub
- How to link issues to pull requests
- How to use both Visual Studio and command line for Git operations
- Best practices for Git workflow

## Prerequisites
- Git installed on your computer
- GitHub account with access to the repository
- Visual Studio or VS Code installed
- Basic understanding of the Budget Buddy project structure

## Understanding Git Workflow

### Why Use Branches?
- **Isolation**: Work on features without affecting the main code
- **Collaboration**: Multiple people can work on different features
- **Safety**: Main branch stays stable while you develop
- **Review**: Changes can be reviewed before merging

### Typical Workflow
1. **Create a branch** for your feature or bug fix
2. **Make changes** and commit them
3. **Push the branch** to GitHub
4. **Create a pull request** to merge into main
5. **Link issues** to track what the PR fixes
6. **Review and merge** the changes

## Method 1: Using Visual Studio (Recommended for Beginners)

### 1. Setting Up Git in Visual Studio

#### First Time Setup
1. **Open Visual Studio**
2. **Go to File → Account Settings**
3. **Sign in to your GitHub account**
4. **Go to Tools → Options → Source Control → Git**
5. **Configure your name and email**:
   - Name: Your full name
   - Email: Your GitHub email address

#### Clone the Repository (if not already done)
1. **Open Visual Studio**
2. **Click "Clone a repository"**
3. **Enter the repository URL**: `https://github.com/tsalright-computer-programming/budget-buddy-app.git`
4. **Choose a local path** for your project
5. **Click "Clone"**

### 2. Creating a Branch

#### Create a New Branch
1. **In Visual Studio**, go to **View → Git Changes**
2. **Click the branch dropdown** (should show "main")
3. **Click "New Branch"**
4. **Enter a branch name** (e.g., `feature/categories-page` or `bugfix/validation-error`)
5. **Click "Create Branch"**

#### Switch Between Branches
1. **Click the branch dropdown** in Git Changes
2. **Select the branch** you want to switch to
3. **Visual Studio will switch** to that branch

### 3. Making Changes and Committing

#### Make Your Code Changes
1. **Edit your files** as needed
2. **Save your changes** (`Ctrl+S`)

#### Stage and Commit Changes
1. **Go to Git Changes** (View → Git Changes)
2. **You'll see your changed files** in the "Changes" section
3. **Review your changes** by clicking on each file
4. **Add a commit message** at the bottom (e.g., "Add Categories page with CRUD operations")
5. **Click "Commit All"** to commit your changes

#### Push Changes to GitHub
1. **After committing**, click **"Push"** in the Git Changes window
2. **Enter your GitHub credentials** if prompted
3. **Your changes will be pushed** to GitHub

### 4. Creating a Pull Request

#### From Visual Studio
1. **After pushing your branch**, you'll see a notification
2. **Click "Create Pull Request"** in the notification
3. **Fill out the PR details**:
   - **Title**: Brief description of your changes
   - **Description**: Detailed explanation of what you changed
   - **Reviewers**: Add team members if needed
4. **Click "Create Pull Request"**

#### From GitHub Website
1. **Go to the repository** on GitHub
2. **You'll see a banner** saying "Compare & pull request"
3. **Click "Compare & pull request"**
4. **Fill out the PR details** and click "Create pull request"

### 5. Linking Issues to Pull Requests

#### In the Pull Request Description
1. **In your PR description**, add one of these:
   - `Fixes #123` (closes the issue when PR is merged)
   - `Closes #123` (same as Fixes)
   - `Resolves #123` (same as Fixes)
   - `Related to #123` (links but doesn't close)

#### Example PR Description
```markdown
## Description
This PR adds the Categories management page with full CRUD operations.

## Changes
- Created CategoriesPage component with list, create, edit, and archive functionality
- Added form validation and error handling
- Implemented API integration for all CRUD operations
- Added responsive table layout with action buttons

## Testing
- [x] Create new categories
- [x] Edit existing categories
- [x] Archive/unarchive categories
- [x] Form validation works correctly
- [x] API errors are displayed properly

Fixes #15
```

## Method 2: Using Command Line (Advanced)

### 1. Setting Up Git

#### Configure Git (First Time Only)
```bash
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

#### Clone the Repository (if not already done)
```bash
git clone https://github.com/tsalright-computer-programming/budget-buddy-app.git
cd budget-buddy-app
```

### 2. Creating a Branch

#### Create and Switch to New Branch
```bash
git checkout -b feature/categories-page
```

#### Switch Between Branches
```bash
git checkout main
git checkout feature/categories-page
```

#### List All Branches
```bash
git branch -a
```

### 3. Making Changes and Committing

#### Check Status
```bash
git status
```

#### Add Files to Staging
```bash
# Add specific files
git add src/pages/CategoriesPage.tsx

# Add all changed files
git add .

# Add all files in a directory
git add src/
```

#### Commit Changes
```bash
git commit -m "Add Categories page with CRUD operations"
```

#### Push Branch to GitHub
```bash
git push origin feature/categories-page
```

### 4. Creating a Pull Request

#### Using GitHub CLI (if installed)
```bash
gh pr create --title "Add Categories Page" --body "This PR adds the Categories management page with full CRUD operations. Fixes #15"
```

#### Using GitHub Website
1. **Go to the repository** on GitHub
2. **Click "Compare & pull request"** banner
3. **Fill out the PR details**

## Best Practices

### Branch Naming
- **Feature branches**: `feature/description` (e.g., `feature/categories-page`)
- **Bug fixes**: `bugfix/description` (e.g., `bugfix/validation-error`)
- **Hotfixes**: `hotfix/description` (e.g., `hotfix/security-patch`)
- **User stories**: `s1.1/category-model` (e.g., `s2.1/transaction-model`)

### Commit Messages
- **Use present tense**: "Add feature" not "Added feature"
- **Be descriptive**: "Add Categories page with CRUD operations" not "Add page"
- **Keep under 50 characters** for the subject line
- **Use body for detailed explanations** if needed

### Pull Request Guidelines
- **Clear title**: Summarize what the PR does
- **Detailed description**: Explain what changed and why
- **Link issues**: Use `Fixes #123` to close issues
- **Add screenshots**: For UI changes
- **Test thoroughly**: Before creating the PR

## Common Git Commands Reference

### Basic Commands
```bash
# Check status
git status

# Add files
git add .
git add filename

# Commit changes
git commit -m "Your message"

# Push to GitHub
git push origin branch-name

# Pull latest changes
git pull origin main

# Switch branches
git checkout branch-name

# Create new branch
git checkout -b new-branch-name

# Merge branch
git merge branch-name

# Delete branch
git branch -d branch-name
```

### Advanced Commands
```bash
# View commit history
git log --oneline

# View changes
git diff

# Undo last commit (keep changes)
git reset --soft HEAD~1

# Undo last commit (lose changes)
git reset --hard HEAD~1

# Stash changes
git stash

# Apply stashed changes
git stash pop

# View remote repositories
git remote -v

# Fetch latest changes
git fetch origin
```

## Troubleshooting

### Issue: "Authentication failed"
**Solution**: 
1. Use GitHub Personal Access Token instead of password
2. Configure Git credentials in Visual Studio
3. Use SSH keys for authentication

### Issue: "Branch not found"
**Solution**: 
1. Make sure you've pushed the branch: `git push origin branch-name`
2. Check branch name spelling
3. Verify you're in the correct repository

### Issue: "Merge conflicts"
**Solution**: 
1. Pull latest changes: `git pull origin main`
2. Resolve conflicts in Visual Studio
3. Commit the resolved changes
4. Push the updated branch

### Issue: "Cannot push to main"
**Solution**: 
1. Create a feature branch instead
2. Push to feature branch
3. Create a pull request
4. Never push directly to main

### Issue: "Changes not showing in PR"
**Solution**: 
1. Make sure you've committed changes: `git commit -m "message"`
2. Push the branch: `git push origin branch-name`
3. Check the correct branch is selected in the PR

## Workflow Examples

### Example 1: Adding a New Feature
```bash
# 1. Create feature branch
git checkout -b feature/transactions-page

# 2. Make changes and commit
git add .
git commit -m "Add Transactions page with filtering"

# 3. Push branch
git push origin feature/transactions-page

# 4. Create PR on GitHub
# 5. Link issue: "Fixes #20"
# 6. Wait for review and merge
```

### Example 2: Fixing a Bug
```bash
# 1. Create bugfix branch
git checkout -b bugfix/validation-error

# 2. Fix the bug and commit
git add .
git commit -m "Fix validation error in Categories form"

# 3. Push branch
git push origin bugfix/validation-error

# 4. Create PR on GitHub
# 5. Link issue: "Fixes #25"
# 6. Wait for review and merge
```

### Example 3: Working on User Story
```bash
# 1. Create story branch
git checkout -b s2.1/transaction-model

# 2. Complete the story and commit
git add .
git commit -m "Complete S2.1: Create Transaction model with migration"

# 3. Push branch
git push origin s2.1/transaction-model

# 4. Create PR on GitHub
# 5. Link issue: "Fixes #12"
# 6. Wait for review and merge
```

## What You've Accomplished

By following this guide, you've learned:
- ✅ **How to create and manage Git branches**
- ✅ **How to commit and push code changes**
- ✅ **How to create pull requests on GitHub**
- ✅ **How to link issues to pull requests**
- ✅ **How to use both Visual Studio and command line**
- ✅ **Best practices for Git workflow**

## Next Steps

After mastering this workflow:
- You'll be able to collaborate effectively on the Budget Buddy project
- You'll understand professional Git practices
- You'll be ready to work on larger projects with teams
- You'll know how to track and manage issues and features

## Key Concepts Learned

### Git Workflow
- Branches isolate different features and fixes
- Commits save snapshots of your work
- Pull requests enable code review and collaboration
- Issues track what needs to be done

### Collaboration
- Never push directly to main branch
- Always create feature branches
- Use descriptive commit messages
- Link issues to track progress

### Best Practices
- Keep commits small and focused
- Write clear commit messages
- Test before creating PRs
- Review code before merging

**Congratulations!** 🎉 You've learned the complete Git workflow for the Budget Buddy project. You're now ready to collaborate effectively and contribute to the project professionally!

## Additional Resources

- [Git Documentation](https://git-scm.com/doc)
- [GitHub Guides](https://guides.github.com/)
- [Visual Studio Git Features](https://docs.microsoft.com/en-us/visualstudio/version-control/)
- [Budget Buddy Repository](https://github.com/tsalright-computer-programming/budget-buddy-app)
