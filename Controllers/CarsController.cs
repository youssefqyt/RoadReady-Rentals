namespace CarRental.Controllers;

using CarRental.Data;
using CarRental.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CarsController : Controller
{
    private readonly AppDbContext _db;

    public CarsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? search, CarCategory? category, decimal? maxPrice, bool? available)
    {
        var query = _db.Cars.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(c => c.Make.Contains(search) || c.Model.Contains(search));

        if (category.HasValue)
            query = query.Where(c => c.Category == category.Value);

        if (maxPrice.HasValue)
            query = query.Where(c => c.PricePerDay <= maxPrice.Value);

        if (available.HasValue)
            query = query.Where(c => c.IsAvailable == available.Value);

        var cars = await query.OrderBy(c => c.PricePerDay).ToListAsync();

        ViewData["Search"] = search;
        ViewData["Category"] = category;
        ViewData["MaxPrice"] = maxPrice;
        ViewData["Available"] = available;
        ViewData["Categories"] = Enum.GetValues<CarCategory>();

        return View(cars);
    }

    public async Task<IActionResult> Details(int id)
    {
        var car = await _db.Cars.FirstOrDefaultAsync(c => c.Id == id);
        if (car == null) return NotFound();
        return View(car);
    }

    [Authorize(Policy = "AdminOnly")]
    public IActionResult Create() => View();

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Car car)
    {
        if (!ModelState.IsValid) return View(car);
        _db.Cars.Add(car);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Edit(int id)
    {
        var car = await _db.Cars.FindAsync(id);
        if (car == null) return NotFound();
        return View(car);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Car car)
    {
        if (id != car.Id) return NotFound();
        if (!ModelState.IsValid) return View(car);
        _db.Update(car);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var car = await _db.Cars.FindAsync(id);
        if (car == null) return NotFound();
        _db.Cars.Remove(car);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
