# DriveEase Car Rental

An advanced ASP.NET Core web application for car rentals, built with .NET 10, Entity Framework Core (SQLite), and Bootstrap 5.

## Features

- **Car catalog** — browse, search, and filter vehicles by make/model, category, price, and availability
- **Car details & booking** — view car specs and submit booking requests with date picker and instant price preview
- **User accounts** — registration, login, and cookie-based authentication (admin & customer roles)
- **Booking management** — view my bookings, cancel pending requests; admins can confirm/cancel bookings
- **Admin dashboard** — fleet stats, revenue, pending bookings count, and full CRUD over cars, bookings, and users
- **Responsive UI** — Bootstrap 5 with Bootstrap Icons, mobile-friendly layout

## Tech Stack

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core + SQLite
- Bootstrap 5.3 + Bootstrap Icons
- PBKDF2 password hashing (built-in .NET cryptography)

## Project Structure

```
ASP.NET/
├── Program.cs              # App entry point, services, middleware
├── ASP.NET.csproj          # Project file & package references
├── Controllers/
│   ├── HomeController.cs   # Home, privacy, error
│   ├── CarsController.cs   # Car catalog + admin CRUD
│   ├── BookingsController.cs# Booking create/cancel/status
│   ├── AccountController.cs# Login/register/logout
│   └── AdminController.cs  # Admin dashboard & management
├── Models/
│   └── Car.cs              # Car, User, Booking, CarCategory
├── Data/
│   └── AppDbContext.cs     # EF context + seed data
├── Services/
│   └── PasswordHasher.cs   # PBKDF2 hashing
├── Views/
│   ├── Shared/_Layout.cshtml
│   ├── Home/, Cars/, Bookings/, Account/, Admin/
├── wwwroot/
│   ├── css/site.css
│   └── js/site.js
└── carrental.db            # SQLite database (created on first run)
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (10.0.x)

### Run

```powershell
# Navigate to the project folder
cd C:\Users\Dell\Desktop\Summer\ASP.NET

# Restore packages and build
dotnet build

# Run the application
dotnet run
```

The app will be available at `http://localhost:5050` (or the port shown in the console output).

### Test Accounts (seeded on first run)

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@carrental.com | Admin123! |
| Customer | john@example.com | Customer123! |

## Database

SQLite database file `carrental.db` is created automatically in the project folder on first launch. Seed data includes 8 cars, an admin user, a customer user, and one sample booking.

To reset the database, delete `carrental.db` (and `carrental.db-wal`/`carrental.db-shm` if present) and restart the app.

## License

MIT