# Budget Buddy - Full Stack Learning Project

## Overview

This is a **training repository** designed to teach full-stack application development using modern web technologies. The project follows a structured learning path that introduces students to backend development, database management, and API design.

## Learning Objectives

By working through this project, students will learn:

- **Backend Development**: Building RESTful APIs with ASP.NET Core
- **Database Management**: Using Entity Framework Core with SQLite
- **API Design**: Creating clean, well-documented endpoints
- **Development Tools**: Command line usage, package management, and debugging
- **Testing**: API endpoint testing and database connectivity verification

## Project Structure

```
Budget-Buddy/
├── Controllers/           # API endpoints
│   ├── HealthController.cs    # Simple health check
│   └── BudgetController.cs    # Database testing endpoint
├── Models/               # Data models (POCOs)
│   └── Category.cs           # Category entity
├── AppDbContext.cs       # Entity Framework context
├── Program.cs            # Application configuration
└── app.db               # SQLite database file
```

## Getting Started

### Prerequisites
- Windows computer
- Visual Studio 2022 Community Edition
- .NET 9.0 SDK (included with Visual Studio)

### Quick Start
1. **First time setup?** Follow the [Tools Installation Guide](docs/install-tools.md) to install all required software
2. Follow the [Database Setup Guide](docs/setup-db-connections.md) for detailed step-by-step instructions
3. Run the application: `dotnet run --project Budget-Buddy/Budget-Buddy.csproj`
4. Test the endpoints in your browser or using the Swagger UI

## Available Endpoints

### Health Check
- **URL**: `GET http://localhost:5044/health`
- **Purpose**: Simple application health verification
- **Response**: `{"status": "ok"}`

### Budget API
- **URL**: `GET http://localhost:5044/budget`
- **Purpose**: Database connectivity test and data retrieval
- **Response**: JSON with database status and category count

### API Documentation
- **URL**: `http://localhost:5044/swagger`
- **Purpose**: Interactive API documentation and testing interface

## Documentation

### 📚 Learning Resources
- [Tools Installation Guide](docs/install-tools.md) - Step-by-step instructions for installing Visual Studio, .NET SDK, and other required tools
- [Creating a Basic Web API](docs/create-web-api.md) - Learn how to create a new ASP.NET Web API project from scratch in Visual Studio
- [Database Setup Instructions](docs/setup-db-connections.md) - Complete beginner-friendly guide for setting up the database and running the application
- [Adding POST Endpoints](docs/add-post-endpoint.md) - Learn how to create POST endpoints for adding new data to your API

### 🔧 Technical Details
- **Framework**: ASP.NET Core 9.0
- **Database**: SQLite with Entity Framework Core
- **API Documentation**: Swagger/OpenAPI
- **Development Environment**: Visual Studio 2022 Community

## Learning Path

### Phase 1: Foundation
- [x] Project setup and configuration
- [x] Basic API endpoint creation
- [x] Database connection setup
- [x] Entity Framework integration

### Phase 2: Development (Coming Soon)
- [ ] CRUD operations for budget items
- [ ] User authentication and authorization
- [ ] Data validation and error handling
- [ ] Frontend integration

### Phase 3: Advanced Features (Future)
- [ ] Real-time updates
- [ ] Data visualization
- [ ] Mobile app integration
- [ ] Deployment and hosting

## Troubleshooting

If you encounter any issues, refer to the [troubleshooting section](docs/setup-db-connections.md#troubleshooting---common-problems-and-solutions) in the database setup guide.

## Contributing

This is a learning repository. Students should:
1. Follow the step-by-step instructions
2. Experiment with the code
3. Ask questions when stuck
4. Document their learning progress

## Support

For questions or issues:
1. Check the troubleshooting guide
2. Take screenshots of error messages
3. Note the exact steps that led to the problem
4. Ask for help with specific details

---

**Happy Learning!** 🚀

This project is designed to build confidence and practical skills in full-stack development. Take your time, experiment, and don't hesitate to ask questions!
