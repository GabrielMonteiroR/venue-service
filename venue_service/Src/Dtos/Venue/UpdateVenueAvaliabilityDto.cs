using System.ComponentModel.DataAnnotations;

namespace venue_service.Src.Dtos.Venue
{
    public class UpdateVenueAvaliabilityDto
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }
    }
}
