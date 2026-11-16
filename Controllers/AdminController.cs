namespace CarRental.Controllers;

using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Policy = "AdminOnly")]
public class AdminController : Controller
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Dashboard()
    {
        var totalCars = await _db.Cars.CountAsync();
        var totalUsers = await _db.Users.CountAsync();
        var totalBookings = await _db.Bookings.CountAsync();
        var pendingBookings = await _db.Bookings.CountAsync(b => b.Status == "Pending");
        var revenue = await _db.Bookings.Where(b => b.Status == "Confirmed").SumAsync(b => (decimal?)b.TotalPrice) ?? 0;

        ViewData["TotalCars"] = totalCars;
        ViewData["TotalUsers"] = totalUsers;
        ViewData["TotalBookings"] = totalBookings;
        ViewData["PendingBookings"] = pendingBookings;
        ViewData["Revenue"] = revenue;

        return View();
    }

    public async Task<IActionResult> ManageCars() => View(await _db.Cars.ToListAsync());

    public async Task<IActionResult> ManageBookings()
    {
        var bookings = await _db.Bookings.Include(b => b.Car).Include(b => b.User).OrderByDescending(b => b.CreatedAt).ToListAsync();
        return View(bookings);
    }

    public async Task<IActionResult> ManageUsers() => View(await _db.Users.ToListAsync());
}
