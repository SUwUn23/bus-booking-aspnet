using BusBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var trips = _context.Trips
                .Include(t => t.Route)
                .Include(t => t.Seats)
                .Where(t => t.DepartureDate >= DateTime.Today)
                .OrderBy(t => t.DepartureDate)
                .Take(10)
                .ToList();

            return View(trips);
        }

        [HttpPost]
        public IActionResult Search(SearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var trips = _context.Trips.Include(t => t.Route).Include(t => t.Seats).Where(t => t.DepartureDate >= DateTime.Today).OrderBy(t => t.DepartureDate).Take(10).ToList();
                return View("Index", trips);
            }

            var tripsResult = _context.Trips
                .Include(t => t.Route)
                .Include(t => t.Seats)
                .Where(t => t.Route.FromCity == model.FromCity && t.Route.ToCity == model.ToCity && t.DepartureDate.Date == model.TravelDate.Date)
                .OrderBy(t => t.DepartureTime)
                .ToList();

            ViewBag.SearchModel = model;
            return View("Index", tripsResult);
        }
    }
}
