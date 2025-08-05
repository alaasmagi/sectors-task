# sectors-task

## Description

* UI language: English
* Development year: **2025**
* Languages and technologies: **Backend: C#, .NET Core, EF Core, ASP.NET MVC; Frontend: TypeScript, React**
* Test assignment for **Helmes AS**

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

ADMIN_USERNAME=admin
ADMIN_KEY_BCRYPT='<admin-password-bcrypt-hash>'

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

### Running the backend with Docker (*The Easy way*)
Docker image [link](https://hub.docker.com/r/alaasmagi/sectors-backend)  

Pull the Docker image from Docker Hub:  
```bash
docker pull alaasmagi/sectors-backend
```

The application should have .env file in the path of current terminal/cmd window and it shoult have following content:
```bash
HOST=<your-db-host>
PORT=<your-db-port>
USER=<your-db-user>
DB=<your-db-name>
DBKEY=<your-db-access-key>

ADMIN_USERNAME=admin
ADMIN_KEY_BCRYPT='<admin-password-bcrypt-hash>'

FRONTENDURL=<your-frontend-client-url>
```

To run the container with .env file:  
```bash
docker run --env-file .env -p 5093:5093 alaasmagi/sectors-backend:latest
```
* user interface can be viewed from the web browser on the address the application provided in the terminal/cmd

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
    public Guid ExternalId { get; set; } = Guid.NewGuid();
    [Required] 
    [MaxLength(128)]
    public string FullName { get; set; } = default!;
    
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
The `BaseEntity` class is not defined in this project, it comes from the NuGet package [AL_AppDev.Base(v1.0.3)](https://www.nuget.org/packages/AL_AppDev.Base/1.0.3), which is published and maintained by myself.


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

* **PersonDto**
```csharp
public class PersonDto(PersonEntity person)
{
    public Guid ExternalId { get; set; } = person.ExternalId;
    public string FullName { get; set; } = person.FullName;
    public SectorEntity? Sector { get; set; } = person.Sector;
    public int SectorId { get; set; } = person.SectorId;
    public bool Agreement { get; set; } = person.Agreement;
}
```

### Database structure
Database was created with Entity Framework using the entities listed above. Below is an image of Database structure.
<img src="https://github.com/user-attachments/assets/e5656827-6fbe-4413-90c2-d8f7ac408240" height="300"/>


Full database dump: [sectors-db-dump](https://github.com/alaasmagi/sectors-task/blob/main/sectors-db-dump.sql)


### API documentation
API documentation is done using Swagger. Below is an image of all available API endpoints in this project's backend applicartion.
![image](https://github.com/user-attachments/assets/a729c66f-be4e-48f1-b29b-f5bccac5fc93)


### User Interface
**The application has two UIs:**
* **Main UI** for regular application users (implemented in TypeScript & React)
<img src="https://github.com/user-attachments/assets/7fd5a198-5666-4b73-afc0-33d1c1958707" height="300"/>
<img src="https://github.com/user-attachments/assets/68bedef5-f7f0-4cea-b97c-60f24186ee75" height="300"/>
<img src="https://github.com/user-attachments/assets/72633e7e-5189-4653-b399-8b01a155ab1a" height="300"/>
<img src="https://github.com/user-attachments/assets/afcc9a37-0db2-4445-8882-0168bafb175a" height="300"/>
<img src="https://github.com/user-attachments/assets/9fdf2615-3d3d-4c21-a980-8f5ba64082da" height="300"/>

* **Admin UI** for managing DB data conviniently (implemented via ASP.NET View)
<img src="https://github.com/user-attachments/assets/0925f989-6224-4987-9abb-67044885400d" height="300"/>
<img src="https://github.com/user-attachments/assets/49d5fbe5-6daf-4d23-968f-6fc90aa4a4aa" height="300"/>
<img src="https://github.com/user-attachments/assets/57da73e0-eada-4e85-9a89-83782a102160" height="200"/>
<img src="https://github.com/user-attachments/assets/f6d268c9-eba5-46a2-b14d-4a4c642c1fcb" height="300"/>

For the **Main UI** customisation is done using Tailwind CSS.  
For the **Admin UI** customisation is done using Bootstrap.

## Improvements & scaling possibilities

### Authentication (frontend side)
* JWT based authentication could be used to make frontend safe to use.

### Authentication (backend side)
* Some sort of authorization could be used to make backend admin UI safe for managing database.

### Further application development
* This sector form app could be just the beginning of a great web application with a lot of functionality. 
