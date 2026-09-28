using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        [Required]
        public string Method { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Status { get; set; } = "Pending";

        public string? TransactionId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
