# TicketLab

A small IT Support / Issue Management web app, built as a learning project
for the IT-utvikler Competence Book (Omega365, team PES).

Users can create tickets, set priority and category, change status, add comments
and see the change history. The project grows step by step, one competence goal at a time.

## Tech stack

| Layer    | Technology                                |
|----------|-------------------------------------------|
| Frontend | Vue 3 + TypeScript + Vite + Bootstrap     |
| Backend  | ASP.NET Core Web API (.NET 10)            |
| Database | SQL Server (`.\SQLEXPRESS`) + EF Core     |

```
Vue frontend  →  REST API (ASP.NET Core)  →  EF Core  →  SQL Server
```

## Project structure

```
backend/    ASP.NET Core Web API
frontend/   Vue 3 app
docs/       Roadmap, test plans, bug reports, reflections
```

## Getting started

Requirements: .NET 10 SDK, SQL Server Express (`.\SQLEXPRESS`), Node.js LTS.

```bash
# Restore the local dotnet-ef tool (once)
dotnet tool restore

# Create / update the database
dotnet ef database update --project backend/TicketLab.Api

# Run the API (http://localhost:5032)
dotnet run --project backend/TicketLab.Api
```

The generated SQL for each migration is kept in `docs/sql/` for reference.

See [docs/roadmap.md](docs/roadmap.md) for the plan and how it maps to the competence goals.
