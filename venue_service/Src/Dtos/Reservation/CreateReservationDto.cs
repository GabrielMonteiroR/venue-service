using System.ComponentModel.DataAnnotations;

namespace venue_service.Src.Dtos.Reservation;

public class CreateReservationDto
{
    [Required]
    public int VenueId { get; set; }

    [Required]
    public int VenueAvailabilityTimeId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int PaymentMethodId { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 1)]
    public string CardToken { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string UserEmail { get; set; }
}
