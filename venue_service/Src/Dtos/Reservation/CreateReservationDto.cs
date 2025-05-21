namespace venue_service.Src.Dtos.Reservation;

public class CreateReservationDto
{
    public int VenueId { get; set; }
    public int VenueAvailabilityTimeId { get; set; }
    public int UserId { get; set; }
    public int PaymentMethodId { get; set; }

    public string CardToken { get; set; }
    public string UserEmail { get; set; }
}