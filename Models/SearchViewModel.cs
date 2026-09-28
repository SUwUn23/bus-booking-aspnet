using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.Models
{
    public class SearchViewModel
    {
        [Required]
        public string FromCity { get; set; } = string.Empty;

        [Required]
        public string ToCity { get; set; } = string.Empty;

        [Required]
        public DateTime TravelDate { get; set; } = DateTime.Today;
    }
}
