# Product Management API

A RESTful API for managing product inventory, built with **ASP.NET Core Web API (.NET 9)**, **Entity Framework Core**, and **SQL Server**. Exposes full CRUD operations and category-based search with automatic OpenAPI documentation via Swagger.

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com)
[![EF Core](https://img.shields.io/badge/EF%20Core-9.0-512BD4)](https://learn.microsoft.com/ef/core)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019%2B-CC2927)](https://www.microsoft.com/sql-server)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI%20v3-85EA2D)](https://swagger.io)

---

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [API Endpoints](#api-endpoints)
- [Product Model](#product-model)
- [Prerequisites](#prerequisites)
- [Setup](#setup)
- [Running the API](#running-the-api)
- [Swagger UI](#swagger-ui)
- [Hosting](#hosting)

---

## Features

- Full CRUD for products: create, read, update, and delete
- Category-based product search
- Model validation via Data Annotations (`[Required]`, `[Range]`, `[MaxLength]`)
- Async/await throughout using `Task`-based methods and `CancellationToken` support
- Entity Framework Core with SQL Server — code-first migrations
- Scoped dependency injection (`IProductService` / `ProductService`)
- Structured logging with `ILogger<ProductService>` (product created, updated, deleted)
- OpenAPI documentation with XML comment integration
- HTTPS redirection

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core Web API (.NET 9) |
| ORM | Entity Framework Core 9.0 |
| Database | SQL Server |
| Documentation | Swashbuckle / OpenAPI 3 |
| Logging | ASP.NET Core built-in `ILogger` |

---

## Project Structure

```
ProductManagementAPI/
├── Controllers/
│   └── ProductsController.cs   # API endpoints
├── Data/
│   └── AppDbContext.cs          # EF Core DbContext
├── Models/
│   └── Product.cs               # Product entity with validation attributes
├── Services/
│   ├── IProductService.cs       # Service interface
│   └── ProductService.cs        # EF Core service implementation
├── Properties/
│   └── launchSettings.json
├── appsettings.json             # Connection string & logging config
├── Program.cs                   # App bootstrap, DI, middleware pipeline
└── ProductManagementAPI.csproj
```

---

## API Endpoints

Base URL: `https://localhost:7155/api/products`

| Method | Endpoint | Description | Success | Error |
|---|---|---|---|---|
| `GET` | `/api/products` | Retrieve all products | `200 OK` | — |
| `GET` | `/api/products/{id}` | Retrieve product by ID | `200 OK` | `404 Not Found` |
| `POST` | `/api/products` | Create a new product | `201 Created` | `400 Bad Request` |
| `PUT` | `/api/products/{id}` | Update an existing product | `200 OK` | `400 Bad Request`, `404 Not Found` |
| `DELETE` | `/api/products/{id}` | Delete a product | `204 No Content` | `404 Not Found` |
| `GET` | `/api/products/search?category={value}` | Search products by category | `200 OK` | `400 Bad Request` |

### Example: Create a Product

**Request**
```http
POST /api/products
Content-Type: application/json

{
  "name": "Wireless Keyboard",
  "category": "Electronics",
  "price": 49.99,
  "stock": 120
}
```

**Response** `201 Created`
```json
{
  "id": 1,
  "name": "Wireless Keyboard",
  "category": "Electronics",
  "price": 49.99,
  "stock": 120,
  "createdDate": "2026-07-04T10:00:00Z",
  "updatedDate": null
}
```

---

## Product Model

| Field | Type | Validation |
|---|---|---|
| `id` | `int` | Auto-generated primary key |
| `name` | `string` | Required, max 200 characters |
| `category` | `string` | Required, max 100 characters |
| `price` | `decimal` | Must be greater than `0.00` |
| `stock` | `int` | Must be greater than or equal to `0` |
| `createdDate` | `DateTime` | Auto-set to UTC on creation |
| `updatedDate` | `DateTime?` | Nullable; set on each update |

---

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server) (2019 or later) or [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads)
- [EF Core CLI tools](https://learn.microsoft.com/ef/core/cli/dotnet)

Install the EF Core CLI tools if not already installed:

```bash
dotnet tool install --global dotnet-ef
```

---

## Setup

**1. Clone the repository**

```bash
git clone https://github.com/<your-username>/ProductManagementAPI.git
cd ProductManagementAPI
```

**2. Configure the connection string**

Open `appsettings.json` and update `DefaultConnection` to point to your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProductDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**3. Restore dependencies**

```bash
dotnet restore
```

**4. Apply database migrations**

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## Running the API

```bash
dotnet run
```

The API will be available at:

| Scheme | URL |
|---|---|
| HTTPS | `https://localhost:7155` |
| HTTP | `http://localhost:5270` |

---

## Swagger UI

Interactive API documentation is available in the **Development** environment at:

```
https://localhost:7155/swagger
```

The Swagger UI lists all endpoints, accepted request bodies, response schemas, and HTTP status codes derived from XML documentation comments in the source.

---

## Hosting

To deploy to a live environment, replace the placeholder below with your hosting URL:

```
https://<your-hosting-url>/api/products
```

**GitHub Repository**

```
https://github.com/<your-username>/ProductManagementAPI
```
