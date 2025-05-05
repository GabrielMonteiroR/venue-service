using System.ComponentModel.DataAnnotations;

namespace venue_service.Src.Dtos
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

        [Required]
        [MaxLength(50, ErrorMessage = "TimeStatus cannot exceed 50 characters.")]
        public string TimeStatus { get; set; }

        [Required]
        public bool IsReserved { get; set; }
    }
}
