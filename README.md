# DorApi

DorApi is a RESTful API built with ASP.NET Core and Entity Framework Core, designed for efficient backend development with JWT authentication and Swagger documentation. This project leverages modern .NET technologies to provide a robust, scalable, and secure service architecture.

---

## Table of Contents

- [Features](#features)
- [Techs Used](#techs-used)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

---

## Features

- Modern ASP.NET Core backend (net8.0)
- Entity Framework Core with SQL Server
- JWT Bearer Authentication for secure APIs
- AutoMapper for object mapping
- Swagger/OpenAPI documentation via Swashbuckle
- Modular architecture: Controllers, Services, Repositories, DTOs
- Ready for deployment and further extensibility

---

## Techs Used

- .NET 8 (net8.0)
- ASP.NET Core
- Entity Framework Core (SQL Server, Design, Tools, Relational)
- AutoMapper
- JWT Bearer Authentication (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Swashbuckle (Swagger/OpenAPI)

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- (Optional) [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/MurtadhaJasim/DorApi.git
   cd DorApi
   ```

2. **Configure the database connection**
   - Update your connection string in `appsettings.json`.

3. **Apply Migrations**
   ```bash
   dotnet ef database update --project Dor/Dor.csproj
   ```

4. **Run the API**
   ```bash
   dotnet run --project Dor/Dor.csproj
   ```

5. **Access Swagger UI**
   - Visit [http://localhost:5000/swagger](http://localhost:5000/swagger) (or the port configured in your launch settings).

---

## Project Structure

```
DorApi/
│
├── Dor/
│   ├── Controllers/      # API controllers
│   ├── Data/             # DbContext and data access configuration
│   ├── Dtos/             # Data Transfer Objects
│   ├── Filters/          # Custom filters/middleware
│   ├── Interfaces/       # Abstractions for dependency injection
│   ├── Mappings/         # AutoMapper profiles
│   ├── Migrations/       # EF Core migration files
│   ├── Models/           # Domain models/entities
│   ├── Repository/       # Repository implementations
│   ├── Services/         # Business logic/services
│   ├── Program.cs        # Entry point
│   ├── Dor.csproj        # Project file
│   └── ...               # Other config and resource files
├── README.md
└── ...
```

[View full Dor folder contents on GitHub.](https://github.com/MurtadhaJasim/DorApi/tree/main/Dor)

---

## API Documentation

- Interactive API documentation is available via Swagger once the project is running.
- Default route: `/swagger`

---

## Contributing

Contributions are welcome! Please open issues or submit pull requests for improvements or bug fixes.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Commit your changes (`git commit -am 'Add some fooBar'`)
4. Push to the branch (`git push origin feature/fooBar`)
5. Create a new Pull Request

---

## License

Distributed under the MIT License. See `LICENSE` for details.
