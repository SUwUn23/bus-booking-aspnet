using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.Models
{
    public class Route
    {
        public int Id { get; set; }

        [Required]
        public string FromCity { get; set; } = string.Empty;

        [Required]
        public string ToCity { get; set; } = string.Empty;

        public decimal BasePrice { get; set; }

        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
