using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.Models
{
    public class Seat
    {
        public int Id { get; set; }

        public int TripId { get; set; }
        public Trip Trip { get; set; } = null!;

        [Required]
        public string SeatCode { get; set; } = string.Empty;

        public bool IsBooked { get; set; }
    }
}
