using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int TripId { get; set; }
        public Trip Trip { get; set; } = null!;

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Required]
        public string BookingCode { get; set; } = string.Empty;

        [Required]
        public string SeatCode { get; set; } = string.Empty;

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        public string? Email { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
