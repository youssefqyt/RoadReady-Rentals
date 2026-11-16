namespace CarRental.Models;

public enum CarCategory
{
    Economy,
    Compact,
    Midsize,
    SUV,
    Luxury,
    Convertible,
    Electric
}

public class Car
{
    public int Id { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public CarCategory Category { get; set; }
    public decimal PricePerDay { get; set; }
    public string ImageUrl { get; set; } = "/images/car-placeholder.svg";
    public string Description { get; set; } = string.Empty;
    public int Seats { get; set; }
    public string Transmission { get; set; } = "Automatic";
    public string FuelType { get; set; } = "Petrol";
    public bool IsAvailable { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Customer";
    public string? Phone { get; set; }
    public string? DrivingLicenseNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

public class Booking
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int UserId { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public int Days => (ReturnDate - PickupDate).Days;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Car Car { get; set; } = null!;
    public User User { get; set; } = null!;
}
