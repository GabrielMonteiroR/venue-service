namespace venue_service.Src.Dtos;

public class CreateReservationDto
{
    public int UserId { get; set; }
    public int VenueId { get; set; }
    public int VenueAvailabilityTimeId { get; set; }
    public int PaymentMethodId { get; set; }
}