🔐 Secure Transaction System

A secure backend API built with ASP.NET Core implementing authentication, authorization, and transaction management using modern best practices.

🚀 Features

🔑 JWT-based Authentication

🔄 Refresh Token mechanism

👤 User Registration & Login

🛡️ Role-based Authorization

💾 Entity Framework Core (Code First)

🧱 Clean Architecture (API, Application, Infrastructure)

📦 Repository & Unit of Work Pattern

🔍 Swagger API testing (basic setup)

🔐 Secure password hashing

🏗️ Project Structure
SecureTransactionSystem/
│
├── API/                # Entry point (Controllers, Middleware)
├── Application/        # Business logic (Services, Interfaces)
├── Infrastructure/     # Data access (EF Core, Repositories)
⚙️ Technologies Used

ASP.NET Core Web API

Entity Framework Core

SQL Server

JWT Authentication

AutoMapper

FluentValidation

🔑 Authentication Flow

User logs in with credentials

Server generates:

Access Token (short-lived)

Refresh Token (long-lived)

Client uses Access Token for API calls

When expired → use Refresh Token to get new Access Token

🧪 API Testing
Using Swagger

Since JWT UI integration is limited in the current setup:

Add header manually:

Authorization: Bearer <your_token>
Using Postman (Recommended)
Authorization: Bearer <your_token>
🗄️ Database Setup
1. Add Migration
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API
2. Update Database
dotnet ef database update --project Infrastructure --startup-project API
🔧 Configuration

Update appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "your-database-connection"
},
"Jwt": {
  "Key": "your-secret-key"
}
▶️ Running the Project
dotnet build
dotnet run --project API

Open:

https://localhost:<port>/swagger
📌 Best Practices Implemented

Separation of concerns (Clean Architecture)

Dependency Injection

Secure token handling

Scalable structure for real-world systems

⚠️ Notes

Swagger JWT UI may not fully work in latest .NET versions

Use Postman for reliable testing

Ensure secrets are not committed to Git

📈 Future Improvements

Add logging (Serilog)

Add global exception middleware

Add refresh token rotation

Add Docker support

Add unit & integration tests

👨‍💻 Author

Mirza Shams

⭐ If you found this useful

Give the repo a ⭐ and share!
