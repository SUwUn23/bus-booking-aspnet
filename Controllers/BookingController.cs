using BusBookingSystem.Data;
using BusBookingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult TripDetail(int id)
        {
            var trip = _context.Trips
                .Include(t => t.Route)
                .Include(t => t.Seats)
                .FirstOrDefault(t => t.Id == id);

            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBooking(int tripId, string seatCode, string customerName, string phone, string? email)
        {
            var trip = await _context.Trips.Include(t => t.Seats).FirstOrDefaultAsync(t => t.Id == tripId);
            if (trip == null)
            {
                return NotFound();
            }

            var seat = trip.Seats.FirstOrDefault(s => s.SeatCode == seatCode);
            if (seat == null || seat.IsBooked)
            {
                TempData["Error"] = "Ghế đã được đặt hoặc không hợp lệ.";
                return RedirectToAction(nameof(TripDetail), new { id = tripId });
            }

            var user = await _userManager.GetUserAsync(User);
            var booking = new Booking
            {
                TripId = tripId,
                UserId = user?.Id,
                BookingCode = "BK" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999),
                SeatCode = seatCode,
                CustomerName = customerName,
                Phone = phone,
                Email = email,
                Amount = trip.Fare,
                Status = "Pending"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Checkout), new { id = booking.Id });
        }

        public IActionResult Checkout(int id)
        {
            var booking = _context.Bookings
                .Include(b => b.Trip)
                .ThenInclude(t => t.Route)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(int id, string paymentMethod)
        {
            var booking = await _context.Bookings
                .Include(b => b.Trip)
                .ThenInclude(t => t.Seats)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = "Confirmed";
            var seat = booking.Trip.Seats.FirstOrDefault(s => s.SeatCode == booking.SeatCode);
            if (seat != null)
            {
                seat.IsBooked = true;
            }

            var payment = new Payment
            {
                BookingId = booking.Id,
                Method = paymentMethod,
                Amount = booking.Amount,
                Status = "Paid",
                TransactionId = "TXN" + DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thanh toán thành công. Vé của bạn đã được xác nhận.";
            return RedirectToAction(nameof(BookingDetail), new { id = booking.Id });
        }

        public async Task<IActionResult> MyBookings()
        {
            var user = await _userManager.GetUserAsync(User);
            var bookings = await _context.Bookings
                .Include(b => b.Trip)
                .ThenInclude(t => t.Route)
                .Where(b => b.UserId == user!.Id)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        public IActionResult BookingDetail(int id)
        {
            var booking = _context.Bookings
                .Include(b => b.Trip)
                .ThenInclude(t => t.Route)
                .FirstOrDefault(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
    }
}
