using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.Models
{
    public class Trip
    {
        public int Id { get; set; }

        public int RouteId { get; set; }
        public Route Route { get; set; } = null!;

        [Required]
        public string BusName { get; set; } = string.Empty;

        [Required]
        public string LicensePlate { get; set; } = string.Empty;

        public DateTime DepartureDate { get; set; }

        public TimeSpan DepartureTime { get; set; }

        public TimeSpan ArrivalTime { get; set; }

        public int TotalSeats { get; set; }

        public decimal Fare { get; set; }

        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public int AvailableSeatsCount() => Seats.Count(s => !s.IsBooked);
    }
}
