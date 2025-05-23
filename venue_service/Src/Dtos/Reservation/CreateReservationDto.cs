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
    [Range(1, 3, ErrorMessage = "Método de pagamento inválido")]
    public int PaymentMethodId { get; set; }

    [StringLength(255, MinimumLength = 1)]
    public string? CardToken { get; set; }

    [EmailAddress]
    [StringLength(255)]
    public string? UserEmail { get; set; }
}
