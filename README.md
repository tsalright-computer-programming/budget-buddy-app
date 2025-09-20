# Budget Buddy - Full Stack Learning Project

## Overview

This is a **comprehensive training repository** designed to teach full-stack application development using modern web technologies. The project follows a structured, user story-driven learning path that takes students from absolute beginner to building a complete budget management application.

## Learning Objectives

By working through this project, students will learn:

- **Backend Development**: Building RESTful APIs with ASP.NET Core
- **Database Management**: Using Entity Framework Core with SQLite
- **API Design**: Creating clean, well-documented endpoints with proper validation
- **Frontend Development**: Building React applications with TypeScript
- **Development Tools**: Visual Studio, VS Code, command line, Git workflow
- **Professional Practices**: Version control, pull requests, issue management
- **Testing**: API endpoint testing, database connectivity, and frontend integration

## Project Structure

```
budget-buddy-app/
├── Budget-Buddy/                    # Backend API (.NET Core)
│   ├── Controllers/                 # API endpoints
│   │   ├── HealthController.cs      # Health check endpoint
│   │   ├── CategoryController.cs    # Category CRUD operations
│   │   └── TransactionsController.cs # Transaction CRUD operations
│   ├── Models/                      # Data models
│   │   ├── Category.cs              # Category entity
│   │   └── Transaction.cs           # Transaction entity
│   ├── DTOs/                        # Data Transfer Objects
│   ├── Migrations/                  # Database migrations
│   ├── AppDbContext.cs              # Entity Framework context
│   └── Program.cs                   # Application configuration
├── budget-buddy-frontend/           # Frontend React App (Future)
├── docs/                           # Comprehensive documentation
│   ├── user-strories/              # User stories (GitHub issues)
│   └── instructions/               # Step-by-step guides
└── README.md                       # This file
```

## Getting Started

### Prerequisites
- **Windows or Mac computer**
- **Visual Studio 2022 Community Edition** (Windows) or **Visual Studio Code** (Mac)
- **.NET 9.0 SDK** (included with Visual Studio or download separately)
- **Node.js 18+** (for frontend development)
- **Git** (for version control)

### Quick Start
1. **First time setup?** Follow the [Tools Installation Guide](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/instructions/S0.1/create-web-api.md) to install all required software
2. **Start with User Stories**: Each user story has detailed step-by-step instructions
3. **Follow the Learning Path**: Work through S0.1 → S0.2 → S0.3 → S1.1 → S1.2 → S2.1 → S2.2 → S2.3 → S3.1 → S3.2 → S3.3
4. **Use GitHub Issues**: Each user story is designed to be a GitHub issue with linked instructions

## Learning Path & User Stories

### **Phase 0: Foundation (S0.1 - S0.3)**
- **[S0.1: Create Web API](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/user-strories/S0.1.md)** - Set up ASP.NET Core project and health endpoint
- **[S0.2: Setup Database](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/user-strories/S0.2.md)** - Configure Entity Framework with SQLite
- **[S0.3: Enable CORS](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/user-strories/S0.3.md)** - Allow React dev server to call the API

### **Phase 1: Categories (S1.1 - S1.2)**
- **[S1.1: Category Model](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/user-strories/S1.1.md)** - Create Category entity and database migration
- **[S1.2: Category CRUD API](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/user-strories/S1.2.md)** - Build complete CRUD endpoints for categories

### **Phase 2: Transactions (S2.1 - S2.3)**
- **[S2.1: Transaction Model](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/user-strories/S2.1.md)** - Create Transaction entity with foreign keys
- **[S2.2: Transaction CRUD API](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/user-strories/S2.2.md)** - Build transaction management endpoints
- **[S2.3: Monthly Summary API](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/user-strories/S2.3.md)** - Create financial reporting endpoints

### **Phase 3: Frontend (S3.1 - S3.3)**
- **[S3.1: React App Setup](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/user-strories/S3.1.md)** - Create React + TypeScript application
- **[S3.2: Categories Page](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/user-strories/S3.2.md)** - Build category management UI
- **[S3.3: Transactions Page](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/user-strories/S3.3.md)** - Build transaction management and reporting UI

## Available Endpoints (After Completion)

### Health Check
- **URL**: `GET http://localhost:5044/health`
- **Purpose**: Application health verification
- **Response**: `{"status": "ok"}`

### Categories API
- **URL**: `GET/POST/PUT/DELETE http://localhost:5044/api/categories`
- **Purpose**: Manage income and expense categories
- **Features**: CRUD operations, validation, soft delete

### Transactions API
- **URL**: `GET/POST/PUT/DELETE http://localhost:5044/api/transactions`
- **Purpose**: Manage financial transactions
- **Features**: Filtering, date ranges, category relationships

### Summary API
- **URL**: `GET http://localhost:5044/api/summary?month=YYYY-MM`
- **Purpose**: Monthly financial summaries
- **Response**: Income, expense, and net totals

### API Documentation
- **URL**: `http://localhost:5044/swagger`
- **Purpose**: Interactive API documentation and testing interface

## Documentation

### 📚 Comprehensive Learning Resources

#### **User Stories & Instructions**
Each user story includes detailed step-by-step instructions with:
- **Beginner-friendly explanations** for every concept
- **Code examples** with full context
- **Troubleshooting guides** for common issues
- **Git workflow recommendations** for professional development
- **Prerequisites** clearly listed for each step

#### **General Guides**
- **[SQLite Setup and Usage Guide](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/instructions/general/sqlite-setup-and-usage.md)** - Complete database management guide
- **[Git Workflow Guide](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/instructions/general/git-workflow-guide.md)** - Professional version control practices

### 🔧 Technical Stack
- **Backend**: ASP.NET Core 9.0 with Entity Framework Core
- **Database**: SQLite for development and testing
- **Frontend**: React 18+ with TypeScript and Vite
- **API Documentation**: Swagger/OpenAPI
- **Development Tools**: Visual Studio 2022 (Windows) / VS Code (Mac)
- **Version Control**: Git with GitHub integration

## Learning Progression

### **Phase 0: Foundation** ✅
- [x] **S0.1**: ASP.NET Core Web API setup
- [x] **S0.2**: Entity Framework with SQLite database
- [x] **S0.3**: CORS configuration for frontend integration

### **Phase 1: Backend API Development** ✅
- [x] **S1.1**: Category model and database migration
- [x] **S1.2**: Category CRUD API with validation

### **Phase 2: Advanced Backend Features** ✅
- [x] **S2.1**: Transaction model with foreign keys
- [x] **S2.2**: Transaction CRUD API with filtering
- [x] **S2.3**: Monthly summary and reporting API

### **Phase 3: Frontend Development** 🚧
- [ ] **S3.1**: React + TypeScript application setup
- [ ] **S3.2**: Category management UI
- [ ] **S3.3**: Transaction management and reporting UI

### **Future Enhancements** 🔮
- [ ] User authentication and authorization
- [ ] Data visualization and charts
- [ ] Mobile app integration
- [ ] Deployment and hosting
- [ ] Real-time updates

## Troubleshooting

### **Common Issues & Solutions**
- **Database Migration Errors**: See troubleshooting sections in S1.1 and S2.1 instruction guides
- **CORS Issues**: Check S0.3 instructions for proper CORS configuration
- **React Setup Problems**: Follow S3.1 troubleshooting section for Node.js and Vite issues
- **Git Workflow Questions**: Refer to the [Git Workflow Guide](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/instructions/general/git-workflow-guide.md)

### **Getting Help**
1. **Check the specific instruction guide** for your current user story
2. **Review the troubleshooting section** in each guide
3. **Take screenshots** of error messages
4. **Note the exact steps** that led to the problem
5. **Ask for help** with specific details and context

## Professional Development Practices

### **Git Workflow**
- **Create feature branches** for each user story
- **Commit frequently** with descriptive messages
- **Create pull requests** for code review
- **Link issues** to pull requests for tracking
- **Follow the [Git Workflow Guide](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/instructions/general/git-workflow-guide.md)** for best practices

### **Code Quality**
- **Follow the step-by-step instructions** exactly
- **Test your endpoints** using Swagger UI
- **Validate your database** using SQLite Browser
- **Experiment and learn** from the code examples
- **Document your progress** as you work through each story

---

## 🎯 **Ready to Start Learning?**

**Begin with [S0.1: Create Web API](https://github.com/tsalright-computer-programming/budget-buddy-app/blob/main/docs/user-strories/S0.1.md)** and follow the complete learning path!

This project is designed to build confidence and practical skills in full-stack development. Each user story builds upon the previous ones, creating a comprehensive learning experience that takes you from beginner to building a complete budget management application.

**Happy Learning!** 🚀
