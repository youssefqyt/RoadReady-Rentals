namespace CarRental.Controllers;

using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class BookingsController : Controller
{
    private readonly AppDbContext _db;

    public BookingsController(AppDbContext db) => _db = db;

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin = User.IsInRole("Admin");

        var query = _db.Bookings.Include(b => b.Car).Include(b => b.User).AsQueryable();
        if (!isAdmin) query = query.Where(b => b.UserId == userId);

        var bookings = await query.OrderByDescending(b => b.CreatedAt).ToListAsync();
        ViewData["IsAdmin"] = isAdmin;
        return View(bookings);
    }

    [Authorize]
    public async Task<IActionResult> Create(int carId)
    {
        var car = await _db.Cars.FindAsync(carId);
        if (car == null || !car.IsAvailable) return NotFound();
        ViewData["Car"] = car;
        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int carId, DateTime pickupDate, DateTime returnDate, string? notes)
    {
        var car = await _db.Cars.FindAsync(carId);
        if (car == null || !car.IsAvailable) return NotFound();

        if (returnDate <= pickupDate)
        {
            ModelState.AddModelError("", "Return date must be after pickup date.");
            ViewData["Car"] = car;
            return View();
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var days = (returnDate - pickupDate).Days;
        var total = days * car.PricePerDay;

        var booking = new Booking
        {
            CarId = carId,
            UserId = userId,
            PickupDate = pickupDate,
            ReturnDate = returnDate,
            TotalPrice = total,
            Status = "Pending",
            Notes = notes,
            CreatedAt = DateTime.UtcNow
        };

        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        TempData["Message"] = $"Booking submitted! Total: ${total:F2} for {days} day(s).";
        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin = User.IsInRole("Admin");

        var booking = await _db.Bookings.FindAsync(id);
        if (booking == null) return NotFound();
        if (!isAdmin && booking.UserId != userId) return Forbid();

        booking.Status = "Cancelled";
        await _db.SaveChangesAsync();

        TempData["Message"] = "Booking cancelled.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var booking = await _db.Bookings.FindAsync(id);
        if (booking == null) return NotFound();

        booking.Status = status;
        await _db.SaveChangesAsync();

        TempData["Message"] = $"Booking status updated to {status}.";
        return RedirectToAction(nameof(Index));
    }
}
