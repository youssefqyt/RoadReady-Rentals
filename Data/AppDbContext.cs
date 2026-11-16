namespace CarRental.Data;

using CarRental.Models;
using CarRental.Services;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Car> Cars => Set<Car>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Booking>()
            .Property(b => b.TotalPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Car>()
            .Property(c => c.PricePerDay)
            .HasPrecision(18, 2);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        var cars = new[]
        {
            new Car { Id = 1, Make = "Toyota", Model = "Yaris", Year = 2023, Category = CarCategory.Economy, PricePerDay = 35m, ImageUrl = "/images/toyota-yaris.jpg", Description = "Fuel-efficient city car, perfect for urban driving.", Seats = 5, Transmission = "Manual", FuelType = "Petrol", IsAvailable = true },
            new Car { Id = 2, Make = "Honda", Model = "Civic", Year = 2024, Category = CarCategory.Compact, PricePerDay = 45m, ImageUrl = "/images/honda-civic.jpg", Description = "Reliable compact sedan with great fuel economy.", Seats = 5, Transmission = "Automatic", FuelType = "Petrol", IsAvailable = true },
            new Car { Id = 3, Make = "Tesla", Model = "Model 3", Year = 2024, Category = CarCategory.Electric, PricePerDay = 85m, ImageUrl = "/images/tesla-model3.jpg", Description = "All-electric performance sedan with autopilot.", Seats = 5, Transmission = "Automatic", FuelType = "Electric", IsAvailable = true },
            new Car { Id = 4, Make = "BMW", Model = "X5", Year = 2023, Category = CarCategory.SUV, PricePerDay = 110m, ImageUrl = "/images/bmw-x5.jpg", Description = "Luxury SUV with spacious interior and off-road capability.", Seats = 7, Transmission = "Automatic", FuelType = "Diesel", IsAvailable = true },
            new Car { Id = 5, Make = "Mercedes", Model = "C-Class", Year = 2024, Category = CarCategory.Luxury, PricePerDay = 140m, ImageUrl = "/images/mercedes-cclass.jpg", Description = "Premium luxury sedan for executive travel.", Seats = 5, Transmission = "Automatic", FuelType = "Petrol", IsAvailable = true },
            new Car { Id = 6, Make = "Ford", Model = "Mustang", Year = 2023, Category = CarCategory.Convertible, PricePerDay = 160m, ImageUrl = "/images/ford-mustang.jpg", Description = "Iconic American convertible for weekend getaways.", Seats = 4, Transmission = "Automatic", FuelType = "Petrol", IsAvailable = true },
            new Car { Id = 7, Make = "Nissan", Model = "Leaf", Year = 2023, Category = CarCategory.Electric, PricePerDay = 55m, ImageUrl = "/images/nissan-leaf.jpg", Description = "Affordable electric hatchback for eco-conscious drivers.", Seats = 5, Transmission = "Automatic", FuelType = "Electric", IsAvailable = true },
            new Car { Id = 8, Make = "Audi", Model = "Q7", Year = 2024, Category = CarCategory.SUV, PricePerDay = 130m, ImageUrl = "/images/audi-q7.jpg", Description = "Premium family SUV with quattro all-wheel drive.", Seats = 7, Transmission = "Automatic", FuelType = "Diesel", IsAvailable = true },
        };

        modelBuilder.Entity<Car>().HasData(cars);

        var users = new[]
        {
            new User { Id = 1, FullName = "Admin User", Email = "admin@carrental.com", PasswordHash = PasswordHasher.Hash("Admin123!"), Role = "Admin", Phone = "0700000000" },
            new User { Id = 2, FullName = "John Doe", Email = "john@example.com", PasswordHash = PasswordHasher.Hash("Customer123!"), Role = "Customer", Phone = "0700000001", DrivingLicenseNumber = "DL12345678" },
        };
        modelBuilder.Entity<User>().HasData(users);

        var bookings = new[]
        {
            new Booking { Id = 1, CarId = 3, UserId = 2, PickupDate = new DateTime(2026, 12, 1), ReturnDate = new DateTime(2026, 12, 5), TotalPrice = 340m, Status = "Confirmed", Notes = "GPS and child seat requested", CreatedAt = new DateTime(2026, 11, 10) },
        };
        modelBuilder.Entity<Booking>().HasData(bookings);
    }
}
