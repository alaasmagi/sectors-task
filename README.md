# sectors-task

## Description

* UI language: English
* Development year: **2025**
* Languages and technologies: **Backend: C#, .NET Core, EF Core, ASP.NET MVC; Frontend: TypeScript, React**
* Test assignment for Helmes AS

## How to run

### Prerequisites

* .NET SDK 9.0
* Node.js
* Modern web browser

The application should have two .env files:  
a) the first one in the root of backend folder `/sectors-backend` and it shoult have following content:
```bash
HOST=<your-db-host>
PORT=<your-db-port>
USER=<your-db-user>
DB=<your-db-name>
DBKEY=<your-db-access-key>

FRONTENDURL=<your-frontend-client-url>
```
b) the second one in the root of frontend folder `/sectors-frontend` and it should have following content:
```bash
VITE_API_URL=<your-backend-api-url>
```
These .env files help keeping sensitive DB access information secure and also giving the flexibility of setting frontend and backend URL-s without actually digging into the source code itself.

### Running the app

After meeting all prerequisites above - 
* backend application can be run via terminal/cmd opened in the root of WebApp folder in the backend folder `/sectors-backend/WebApp` by command
```bash
dotnet run
```
* frontend application can be run via terminal/cmd opened in the root of frontend folder `/sectors-frontend` by command
```bash
npm i; npm run dev 
```

## Features
* Users can add their name, job sector and agreement to terms
* Users can manage (edit/delete) their data in the current session

## Design choices

### Backend design
I used ASP.NET MVC to maintain a clean and well-structured codebase. The MVC pattern promotes separation of concerns, which improves code maintainability and enhances testability.

### Frontend design
By separating UI components, views, and business logic, the frontend remains clean, modular, and easy to understand.

### Services
There is one services in the backend:
* **PersonService** - main service that handles all CRUD operations with PersonEntity and also fetches all SectorEntities.

### Domain
Different entities are used to manage data more easily.

* **PersonEntity** - groups all person related data fields together
```csharp
public class  PersonEntity : BaseEntity
{
    [Required] 
    [MaxLength(128)]
    public string FullName { get; set; } = default!;

    // Navigation property, EF handles it automatically
    public SectorEntity? Sector { get; set; } 
    public int SectorId { get; set; }

    [Required]
    public bool Agreement { get; set; }
}
```

* **SalaryInputModel** - groups all sector related data fields together
```csharp
public class SectorEntity : BaseEntity
{
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;

    [JsonIgnore]
    // Navigation property, EF handles it automatically 
    public SectorEntity? Parent { get; set; }
    public int? ParentId { get; set; }

    // Navigation property, EF handles it automatically
    public List<SectorEntity>? Children { get; set; } = [];
}
```

* **BaseEntity** - groups all metadata together
```csharp
public class BaseEntity
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(128)]
    public string CreatedBy { get; set; } = default!;

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    [MaxLength(128)]
    public string UpdatedBy { get; set; } = default!;

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    public bool Deleted { get; set; } = false;
}
```
The `BaseEntity` class is not defined in this project, it comes from the NuGet package [AL_AppDev.Base(v1.0.2)](https://www.nuget.org/packages/AL_AppDev.Base/1.0.2), maintained and published by myself.


### DTOs
One DTO is used in the backend, to optimize SectorEntities' HTTP request payload size.
* **SectorDto**
```csharp
public class SectorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public List<SectorDto>? Children { get; set; }
}
```

### Database structure
Database was created with Entity Framework using the entities listed above. Below is an image of Database structure.
![drawSQL-image-export-2025-05-05 (1)](https://github.com/user-attachments/assets/e5656827-6fbe-4413-90c2-d8f7ac408240)
<img src="https://github.com/user-attachments/assets/e5656827-6fbe-4413-90c2-d8f7ac408240" height="300"/>


Full database dump: [sectors-db-dump](https://github.com/alaasmagi/sectors-task/blob/main/sectors-db-dump.sql)


### API documentation
API documentation is done using Swagger. Below is an image of all available API endpoints in this project's backend applicartion.
![image](https://github.com/user-attachments/assets/a729c66f-be4e-48f1-b29b-f5bccac5fc93)


### User Interface
**The application has two UIs:**
* the main UI for regular application users (implemented in TypeScript & React),
* the admin UI for managing DB data conviniently (implemented via ASP.NET View)

For the **Main UI** customisation is done using Tailwind CSS.
For the **Admin UI** customisation is done using Bootstrap.

## Improvements & scaling possibilities

### Authentication (frontend side)
* JWT based authentication could be used to make frontend safe to use.

### Authentication (backend side)
* Some sort of authorization could be used to make backend admin UI safe for managing database.

### Further application development
* This sector form app could be just the beginning of a great web application with a lot of functionality. 
