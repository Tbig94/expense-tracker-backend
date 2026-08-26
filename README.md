# Expense Tracker – Backend

A modern .NET-based REST API that serves as the server-side (backend) component of the [Expense Tracker](https://github.com/Tbig94/ExpenseTracker) application. The goal is to provide a simple yet well-structured service that allows users to track their income and expenses, categorize them, and get an overview of their finances.

> **Related repositories**
> - Frontend: [Tbig94/ExpenseTracker](https://github.com/Tbig94/ExpenseTracker)
> - Backend (this repo): [Tbig94/expense-tracker-backend](https://github.com/Tbig94/expense-tracker-backend)

---

## Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Main Endpoints](#main-endpoints)
- [License](#license)

---

## Features

- **User management and authentication** – registration, login, JWT (access/refresh token) based authentication
- **Expense management** – full CRUD operations: create, list, update, delete
- **Income management** – full CRUD operations for tracking income
- **Category management** – create and edit custom categories, default category set
- **Filtering and search** – filter transactions by date, category, type (income/expense), and amount
- **Summaries / reports** – monthly/yearly summaries, category-based breakdown, balance calculation
- **Pagination and sorting** – efficient handling of larger datasets
- **Validation and unified error handling** – consistent, structured error responses across all endpoints
- **Authorization** – users can only access their own data
- **Swagger / OpenAPI documentation** – interactive API exploration and testing in the development environment

---

## Tech Stack

| Layer | Technology |
|---|---|
| Language / runtime | C# / .NET (ASP.NET Core Web API) |
| API style | RESTful API, JSON |
| Data access | Entity Framework Core (Code-First migrations) |
| Database | SQL Server / PostgreSQL (configurable via connection string) |
| Authentication | JWT Bearer Token |
| API documentation | Swagger / Swashbuckle (OpenAPI) |
| Object mapping | AutoMapper (DTO ↔ Entity conversion) |
| Validation | FluentValidation / DataAnnotations |
| CI | GitHub Actions (automated build and test on every push/PR) |
| Solution format | `.slnx` (the new, XML-based .NET solution file format) |

---

## Main Endpoints

| Method | Endpoint | Description | Auth required |
|---|---|---|---|
| POST | `/api/auth/register` | Register a new user | No |
| POST | `/api/auth/login` | Log in, issue a JWT token | No |
| POST | `/api/auth/refresh` | Refresh the access token | Yes (refresh token) |
| GET | `/api/expenses` | List expenses (with filtering, pagination) | Yes |
| POST | `/api/expenses` | Create a new expense | Yes |
| PUT | `/api/expenses/{id}` | Update an expense | Yes |
| DELETE | `/api/expenses/{id}` | Delete an expense | Yes |
| GET | `/api/incomes` | List incomes | Yes |
| POST | `/api/incomes` | Create a new income | Yes |
| GET | `/api/categories` | Retrieve categories | Yes |
| POST | `/api/categories` | Create a new category | Yes |
| GET | `/api/reports/summary` | Income/expense summary for a given period | Yes |

---

## License

The license terms of the project are defined in the `LICENSE` file in the repository (if present). If the file is not currently included in the repository, it is advisable to add a suitable open-source license (e.g., MIT).

---

