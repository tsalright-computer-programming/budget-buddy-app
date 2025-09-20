# S1.1 - Refactoring Diagram

## Database Schema Evolution

```mermaid
erDiagram
    Category_Before {
        int Id PK
        string Name
        string Type
    }
    
    Category_After {
        Guid Id PK
        string Name
        CategoryType Type
        bool IsArchived
        DateTime CreatedUtc
    }
    
    CategoryType {
        int Value
        string Name
    }
    
    Category_After ||--|| CategoryType : "Type references"
```

## Refactoring Flow

```mermaid
flowchart TD
    A[Current State] --> B[Create CategoryType Enum]
    B --> C[Refactor Category Model]
    C --> D[Update AppDbContext]
    D --> E[Add Unique Index]
    E --> F[Create Migration]
    F --> G[Apply Migration]
    G --> H[Verify Changes]
    
    A1[Simple Category Model] --> A
    A2[BudgetController for Testing] --> A
    A3[Basic DbContext] --> A
    
    H1[Proper Category Model] --> H
    H2[Type Safety with Enum] --> H
    H3[Unique Constraints] --> H
    H4[Audit Fields] --> H
```

## Controller Refactoring (S1.2)

```mermaid
flowchart LR
    A[BudgetController] --> B[Delete BudgetController]
    B --> C[Create CategoryController]
    C --> D[Implement CRUD Operations]
    
    D --> E[POST /api/category]
    D --> F[GET /api/category]
    D --> G[PUT /api/category/{id}]
    D --> H[DELETE /api/category/{id}]
    
    E --> I[Create Category]
    F --> J[List Categories]
    G --> K[Update Category]
    H --> L[Soft Delete Category]
```

## API Endpoint Structure

```mermaid
graph TD
    A[CategoryController] --> B[POST /api/category]
    A --> C[GET /api/category]
    A --> D[GET /api/category/{id}]
    A --> E[PUT /api/category/{id}]
    A --> F[DELETE /api/category/{id}]
    
    B --> B1[Create Category]
    B --> B2[Validation]
    B --> B3[Unique Check]
    
    C --> C1[List Categories]
    C --> C2[Filter by Type]
    C --> C3[Include Archived]
    
    D --> D1[Get Single Category]
    D --> D2[404 if Not Found]
    
    E --> E1[Update Category]
    E --> E2[Validation]
    E --> E3[Unique Check]
    
    F --> F1[Soft Delete]
    F --> F2[Set IsArchived = true]
```

## Data Flow

```mermaid
sequenceDiagram
    participant Client
    participant Controller
    participant DTO
    participant Model
    participant Database
    
    Client->>Controller: POST /api/category
    Controller->>DTO: Validate CategoryCreateDto
    DTO-->>Controller: Validation Result
    Controller->>Database: Check Uniqueness
    Database-->>Controller: Uniqueness Result
    Controller->>Model: Create Category Entity
    Model->>Database: Save Category
    Database-->>Model: Save Result
    Model-->>Controller: Created Category
    Controller->>DTO: Convert to CategoryReadDto
    DTO-->>Controller: Read DTO
    Controller-->>Client: 201 Created + CategoryReadDto
```

## Error Handling Flow

```mermaid
flowchart TD
    A[API Request] --> B{Validation}
    B -->|Valid| C{Uniqueness Check}
    B -->|Invalid| D[400 Bad Request]
    C -->|Unique| E[Process Request]
    C -->|Duplicate| F[400 Bad Request]
    E --> G{Entity Exists?}
    G -->|Yes| H[Success Response]
    G -->|No| I[404 Not Found]
    
    D --> J[ProblemDetails Response]
    F --> J
    I --> J
    H --> K[Data Response]
```
