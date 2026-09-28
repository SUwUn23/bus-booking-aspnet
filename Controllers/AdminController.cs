using BusBookingSystem.Data;
using BusBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var recentBookings = _context.Bookings
                .Include(b => b.Trip)
                .ThenInclude(t => t.Route)
                .OrderByDescending(b => b.CreatedAt)
                .Take(10)
                .ToList();

            var revenue = _context.Bookings
                .Where(b => b.Status == "Confirmed")
                .Sum(b => (decimal?)b.Amount) ?? 0m;

            var trips = _context.Trips.Count();
            var bookings = _context.Bookings.Count();

            ViewBag.TotalRevenue = revenue;
            ViewBag.TotalTrips = trips;
            ViewBag.TotalBookings = bookings;
            ViewBag.RecentBookings = recentBookings;

            return View();
        }

        public IActionResult Routes()
        {
            var routes = _context.Routes.Include(r => r.Trips).ToList();
            return View(routes);
        }

        public IActionResult Trips()
        {
            var trips = _context.Trips
                .Include(t => t.Route)
                .Include(t => t.Seats)
                .OrderBy(t => t.DepartureDate)
                .ToList();

            return View(trips);
        }

        public IActionResult Bookings()
        {
            var bookings = _context.Bookings
                .Include(b => b.Trip)
                .ThenInclude(t => t.Route)
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            return View(bookings);
        }
    }
}
